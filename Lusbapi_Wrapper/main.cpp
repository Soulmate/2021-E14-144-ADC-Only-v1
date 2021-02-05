#pragma managed
// ВЗЯТО ИЗ "D:\2020 07 lcard puddle\E14-140\Examples\Borland C++ 5.02\ReadData\ReadData.cpp"
//******************************************************************************
// Модуль E14-140.
// Консольная программа с организацией потокового ввода данных с АЦП
// с одновременной записью получаемых данных на диск в реальном масштабе времени.
// Ввод осуществляется с первых четырёх каналов АЦП на частоте 100 кГц.
//******************************************************************************
#include <stdio.h>
#include <conio.h>
#include "Lusbapi.h"

//#define CHANNELS_QUANTITY			(0x4)
int CHANNELS_QUANTITY = 0x4; //2021

// аварийный выход из программы
void AbortProgram(char *ErrorString, bool AbortionFlag = true);
// функция потока ввода данных с АЦП
DWORD WINAPI ServiceReadThread(PVOID /*Context*/);
// ожидание завершения асинхронного запроса на ввод данных
BOOL WaitingForRequestCompleted(OVERLAPPED *ReadOv);
// функция вывода сообщений с ошибками
void ShowThreadErrorMessage(void);

// максимально возможное кол-во опрашиваемых 
// виртуальных слотов (при поиске модуля)
const WORD MaxVirtualSoltsQuantity = 127;

// идентификатор файла
HANDLE hFile;

// идентификатор потока сбора данных
HANDLE hReadThread;
DWORD ReadTid;

// версия библиотеки
DWORD DllVersion;
// указатель на интерфейс модуля
ILE140 *pModule;
// дескриптор устройства
HANDLE ModuleHandle;
// название модуля
char ModuleName[7];
// скорость работы шины USB
BYTE UsbSpeed;
// структура с полной информацией о модуле
MODULE_DESCRIPTION_E140 ModuleDescription;
// структура параметров работы АЦП модуля
ADC_PARS_E140 ap;

// кол-во получаемых отсчетов (кратное 32) для Ф. ReadData()
//DWORD DataStep = 64*1024;
DWORD DataStep = 128;
// будем собирать NDataBlock блоков по DataStep отсчётов в каждом
const WORD NDataBlock = 80;
// частота работы АЦП в кГц     //// от 0,122 до 100
double AdcRate = 10.0;

//Максимальное входное напряжение
int ADC_INPUT_RANGE;

//отсчет выходного напряжения ЦАП
SHORT DacSample0;

// буфер данных
SHORT *ReadBuffer;


//выходной буффер
const int ReadBuffer_Size = 32; //в DataStep'ах
int ReadBuffer_lastWritten = -1;
int ReadBuffer_lastRead = -1;

// флажок завершения работы потока сбора данных
bool IsReadThreadComplete;
// номер ошибки при выполнении сбора данных
WORD ReadThreadErrorNumber;

// экранный счетчик-индикатор
DWORD Counter = 0x0, OldCounter = 0xFFFFFFFF;



// состояния цифровых линий
WORD TtlOut;



void InitializeModule(void)
{
	WORD i;
//	WORD DacSample;

	// сбросим флажок завершения потока ввода данных
	IsReadThreadComplete = false;
	// пока ничего не выделено под буфер данных
	ReadBuffer = NULL;
	// пока не создан поток ввода данных
	hReadThread = NULL;
	// пока откытого файла нет :(
	hFile = INVALID_HANDLE_VALUE;
	// сбросим флаг ошибок потока ввода данных
	ReadThreadErrorNumber = 0x0;

	// зачистим экран монитора	
	//system("cls");

	printf(" *******************************\n");
	printf(" Module E14-140                 \n");
	printf(" Console example for ADC Stream \n");
	printf(" *******************************\n\n");

	// проверим версию используемой библиотеки Lusbapi.dll
	if((DllVersion = GetDllVersion()) != CURRENT_VERSION_LUSBAPI)
	{
		char String[128];
		printf(String, " Lusbapi.dll Version Error!!!\n   Current: %1u.%1u. Required: %1u.%1u",
											DllVersion >> 0x10, DllVersion & 0xFFFF,
											CURRENT_VERSION_LUSBAPI >> 0x10, CURRENT_VERSION_LUSBAPI & 0xFFFF);

		AbortProgram(String);
	}
	else printf(" Lusbapi.dll Version --> OK\n");

	// попробуем получить указатель на интерфейс
	pModule = static_cast<ILE140 *>(CreateLInstance("e140"));
	if(!pModule) AbortProgram(" Module Interface --> Bad\n");
	else printf(" Module Interface --> OK\n");

	// попробуем обнаружить модуль E14-140 в первых 256 виртуальных слотах
	for(i = 0x0; i < MaxVirtualSoltsQuantity; i++) if(pModule->OpenLDevice(i)) break;
	// что-нибудь обнаружили?
	if(i == MaxVirtualSoltsQuantity) AbortProgram(" Can't find any module E14-140 in first 127 virtual slots!\n");
	else printf(" OpenLDevice(%u) --> OK\n", i);

	// попробуем прочитать дескриптор устройства
	ModuleHandle = pModule->GetModuleHandle();
	if(ModuleHandle == INVALID_HANDLE_VALUE) AbortProgram(" GetModuleHandle() --> Bad\n");
	else printf(" GetModuleHandle() --> OK\n");

	// прочитаем название модуля в обнаруженном виртуальном слоте
	if(!pModule->GetModuleName(ModuleName)) AbortProgram(" GetModuleName() --> Bad\n");
	else printf(" GetModuleName() --> OK\n");
	// проверим, что это 'E14-140'
	if(strcmp(ModuleName, "E140")) AbortProgram(" The module is not 'E14-140'\n");
	else printf(" The module is 'E14-140'\n");

	// попробуем получить скорость работы шины USB
	if(!pModule->GetUsbSpeed(&UsbSpeed)) AbortProgram(" GetUsbSpeed() --> Bad\n");
	else printf(" GetUsbSpeed() --> OK\n");
	// теперь отобразим скорость работы шины USB
	printf("   USB is in %s\n", UsbSpeed ? "High-Speed Mode (480 Mbit/s)" : "Full-Speed Mode (12 Mbit/s)");

	// получим информацию из ППЗУ модуля
	if(!pModule->GET_MODULE_DESCRIPTION(&ModuleDescription)) AbortProgram(" GET_MODULE_DESCRIPTION() --> Bad\n");
	else printf(" GET_MODULE_DESCRIPTION() --> OK\n");

	// получим текущие параметры работы АЦП
	if(!pModule->GET_ADC_PARS(&ap)) AbortProgram(" GET_ADC_PARS() --> Bad\n");
	else printf(" GET_ADC_PARS() --> OK\n");
	// установим желаемые параметры работы АЦП
	ap.ClkSource = INT_ADC_CLOCK_E140;							// внутренний запуск АЦП
	ap.EnableClkOutput = ADC_CLOCK_TRANS_DISABLED_E140; 	// без трансляции тактовых импульсо АЦП
	ap.InputMode = NO_SYNC_E140;									// без синхронизации ввода данных
	ap.ChannelsQuantity = CHANNELS_QUANTITY; 					// кол-во активных каналов



	// формируем управляющую таблицу 
	for(i = 0x0; i < ap.ChannelsQuantity; i++) ap.ControlTable[i] = (WORD)(i | (ADC_INPUT_RANGE << 0x6));
	ap.AdcRate = AdcRate;								// частота работы АЦП в кГц
	ap.InterKadrDelay = 0.0;							// межкадровая задержка в мс
	// передадим требуемые параметры работы АЦП в модуль
	if(!pModule->SET_ADC_PARS(&ap)) AbortProgram(" SET_ADC_PARS() --> Bad\n");
	else printf(" SET_ADC_PARS() --> OK\n");

		
	// отобразим параметры сбора данных модуля на экране монитора
	printf(" \n");
	printf(" Module E14-140 (S/N %s) is ready ... \n", ModuleDescription.Module.SerialNumber);
	printf("   Module Info:\n");
	printf("     Module  Revision   is '%c'\n", ModuleDescription.Module.Revision);
	printf("     MCU Driver Version is %s (%s)\n", ModuleDescription.Mcu.Version.Version, ModuleDescription.Mcu.Version.Date);
	printf("   Adc parameters:\n");
	printf("     ChannelsQuantity = %2d\n", ap.ChannelsQuantity);
	printf("     AdcRate = %8.3f kHz\n", ap.AdcRate);
	printf("     InterKadrDelay = %2.4f ms\n", ap.InterKadrDelay);
	printf("     KadrRate = %8.3f kHz\n", ap.KadrRate);

	// проинициализируем состояния выходных цифровых линий
	TtlOut = 0x0;
	pModule->ENABLE_TTL_OUT(true);
}

void RunReading()
{
	// выделим память под буфер
	ReadBuffer = new SHORT[ReadBuffer_Size*DataStep];
	if(!ReadBuffer) AbortProgram(" Can not allocate memory\n");

	// Создаём и запускаем поток сбора данных
	printf(" \n");
	hReadThread = CreateThread(0, 0x2000, ServiceReadThread, 0, 0, &ReadTid);
	if(!hReadThread) AbortProgram(" ServiceReadThread() --> Bad\n");
	else printf(" ServiceReadThread() --> OK\n");	
}

//------------------------------------------------------------------------
// Поток, в котором осуществляется сбор данных
//------------------------------------------------------------------------
DWORD WINAPI ServiceReadThread(PVOID /*Context*/)
{
	WORD i;
	WORD RequestNumber;
//	DWORD FileBytesWritten;
	// массив OVERLAPPED структур из двух элементов
	OVERLAPPED ReadOv[2];
	// массив структур с параметрами запроса на ввод/вывод данных
	IO_REQUEST_LUSBAPI IoReq[2];

	// остановим работу АЦП и одновременно сбросим USB-канал чтения данных
	if(!pModule->STOP_ADC()) { ReadThreadErrorNumber = 0x1; IsReadThreadComplete = true; return 0x0; }

	// формируем необходимые для сбора данных структуры
	for(i = 0x0; i < 0x2; i++)
	{
		// инициализация структуры типа OVERLAPPED
		ZeroMemory(&ReadOv[i], sizeof(OVERLAPPED));
		// создаём событие для асинхронного запроса
		ReadOv[i].hEvent = CreateEvent(NULL, FALSE , FALSE, NULL);
		// формируем структуру IoReq		
		IoReq[i].Buffer = ReadBuffer + i*DataStep;
		IoReq[i].NumberOfWordsToPass = DataStep;
		IoReq[i].NumberOfWordsPassed = 0x0;
		IoReq[i].Overlapped = &ReadOv[i];
		IoReq[i].TimeOut = (DWORD)(DataStep/ap.AdcRate + 1000);
	}

	// делаем предварительный запрос на ввод данных
	RequestNumber = 0x0;
	if(!pModule->ReadData(&IoReq[RequestNumber])) { CloseHandle(ReadOv[0].hEvent); CloseHandle(ReadOv[1].hEvent); ReadThreadErrorNumber = 0x2; IsReadThreadComplete = true; return 0x0; }


	// запустим АЦП
	if(pModule->START_ADC())
	{
		// цикл сбора данных		
		while(true)
		{
			// сделаем запрос на очередную порцию данных
			RequestNumber ^= 0x1;
			int positionToWrite = (ReadBuffer_lastWritten + 2)%ReadBuffer_Size;
			
			
			IoReq[RequestNumber].Buffer = ReadBuffer + positionToWrite * DataStep;		

			if(!pModule->ReadData(&IoReq[RequestNumber])) { ReadThreadErrorNumber = 0x2; break; }
			if(ReadThreadErrorNumber) break;

			// ждём завершения операции сбора предыдущей порции данных
			if(!WaitingForRequestCompleted(IoReq[RequestNumber^0x1].Overlapped)) break;
			if(ReadThreadErrorNumber) break;


			if (*IoReq[RequestNumber^0x1].Buffer == -23131)
				printf("aasdf\n");
			ReadBuffer_lastWritten = (IoReq[RequestNumber^0x1].Buffer - ReadBuffer)/DataStep;

			if(ReadThreadErrorNumber) break;
			else Sleep(20);
			Counter++;
		}
	}
	else { ReadThreadErrorNumber = 0x6; }

	// остановим работу АЦП
	if(!pModule->STOP_ADC()) ReadThreadErrorNumber = 0x1;
	// прервём возможно незавершённый асинхронный запрос на приём данных
	if(!CancelIo(ModuleHandle)) { ReadThreadErrorNumber = 0x7; }
	// освободим все идентификаторы событий
	for(i = 0x0; i < 0x2; i++) CloseHandle(ReadOv[i].hEvent);
	// небольшая задержка
	Sleep(100);
	// установим флажок завершения работы потока сбора данных
	IsReadThreadComplete = true;
	// теперь можно спокойно выходить из потока
	return 0x0;
}

//---------------------------------------------------------------------------
//
//---------------------------------------------------------------------------
BOOL WaitingForRequestCompleted(OVERLAPPED *ReadOv)
{
	DWORD ReadBytesTransferred;

	while(TRUE)
	{
		if(GetOverlappedResult(ModuleHandle, ReadOv, &ReadBytesTransferred, FALSE)) break;
		else if(GetLastError() !=  ERROR_IO_INCOMPLETE) { ReadThreadErrorNumber = 0x3; return FALSE; }
		//else if(kbhit()) { ReadThreadErrorNumber = 0x5; return FALSE; }
		else Sleep(20);
	}
	return TRUE;
}

//------------------------------------------------------------------------
// Отобразим сообщение с ошибкой
//------------------------------------------------------------------------
void ShowThreadErrorMessage(void)
{
	switch(ReadThreadErrorNumber)
	{
		case 0x1:
			printf(" ADC Thread: STOP_ADC() --> Bad\n");
			break;

		case 0x2:
			printf(" ADC Thread: ReadData() --> Bad\n");
			break;

		case 0x3:
			printf(" ADC Thread: Timeout is occured!\n");
			break;

		case 0x4:
			printf(" ADC Thread: Writing data file error!\n");
			break;

		case 0x5:
			// если программа была злобно прервана, предъявим ноту протеста
			printf(" ADC Thread: The program was terminated!\n");
			break;

		case 0x6:
			printf(" ADC Thread: START_ADC() --> Bad\n");
			break;

		case 0x7:
			printf(" ADC Thread: Can't cancel ending input and output (I/O) operations!\n");
			break;

		default:
			printf(" Unknown error!\n");
			break;
	}

	return;
}

//------------------------------------------------------------------------
// аварийное завершение программы
//------------------------------------------------------------------------
void AbortProgram(char *ErrorString, bool AbortionFlag)
{
	//выставляем нулевое напряжение на ЦАП
	SHORT DacSample0 = (SHORT)(0x0);
	if(!pModule->DAC_SAMPLE(&DacSample0, WORD(0x0))) printf(" DAC_SAMPLE(0) --> Bad\n");

	// подчищаем интерфейс модуля
	if(pModule)
	{
		// освободим интерфейс модуля
		if(!pModule->ReleaseLInstance()) printf(" ReleaseLInstance() --> Bad\n");
		else printf(" ReleaseLInstance() --> OK\n");
		// обнулим указатель на интерфейс модуля
		pModule = NULL;
	}

	// освободим память буфера
	if(ReadBuffer) { delete[] ReadBuffer; ReadBuffer = NULL; }
	// освободим идентификатор потока сбора данных
	if(hReadThread) { CloseHandle(hReadThread); hReadThread = NULL; }
	// освободим идентификатор файла данных
	if(hFile != INVALID_HANDLE_VALUE) { CloseHandle(hFile); hFile = INVALID_HANDLE_VALUE; }

	// выводим текст сообщения
	if(ErrorString) printf(ErrorString);

	// прочистим очередь клавиатуры
	//if(kbhit()) { while(kbhit()) getch(); }

	// если нужно - аварийно завершаем программу
	if(AbortionFlag) 
		if(ErrorString) throw ErrorString;
		//exit(0x1);
	// или спокойно выходим из функции   
	else return;
}


//Выставляет напряжение на выходе ЦАП (в Волтах)
bool SetVoltage(double U)
{	
	DacSample0 = (SHORT)((2047 * U/DAC_OUTPUT_RANGE_E140 + ModuleDescription.Dac.OffsetCalibration[0])*ModuleDescription.Dac.ScaleCalibration[0]);

	if(!pModule->DAC_SAMPLE(&DacSample0, WORD(0x0)))
	{
		printf(" DAC_SAMPLE(0) --> Bad\n");
		return false;
	}
	return true;
}

//выставляет значение первых двух ТТЛ выходов (из "D:\2020 07 lcard puddle\E14-140\Examples\Borland C++ 5.02\DigitalIo\DigitalIo.cpp")
bool SetTTL(bool s1, bool s2) 
{
	WORD TtlOut_1 = (WORD)s1;
	WORD TtlOut_2 = (WORD)s2 << 1;
	TtlOut = TtlOut_1 | TtlOut_2;
	

	if (pModule->TTL_OUT(TtlOut))
	{
		printf(" Digital Lines States:     0x%04X\r", TtlOut);
		return true;
	}
	else
	{
		printf("\n\n  TTL_OUT() --> Bad\n");	
		return false;
	}
}


#pragma managed
namespace LusbApi_Wrapper
{
	public ref class Module : System::IDisposable
	{
	public: 
		bool emulationMode;
		double emulationVoltage;
		double emulationT;

		double encoderEmulationOutputVoltage = 0;

		bool s1_current = false;
		bool s2_current = false;

		//Вычитать из кольцевого буфера массив отсчетов
		array<double>^ ReadOutVoltageArray () 
		{ 	
			if (!emulationMode)
			{
				int resultSize = ReadBuffer_lastWritten - ReadBuffer_lastRead;
				if (resultSize < 0)
					resultSize += ReadBuffer_Size;

				array<double>^ result = gcnew array<double>(resultSize*DataStep);
				for (int i =0; i < resultSize; i++)	
				{
					ReadBuffer_lastRead = (ReadBuffer_lastRead + 1)%ReadBuffer_Size;
					for(int t = 0; t<DataStep; t++)
					{
						//result[i*DataStep+t] = (double)((10.0*(double)ReadBuffer[ReadBuffer_lastRead*DataStep+t]/8000.0 + ModuleDescription.Adc.OffsetCalibration[0])*ModuleDescription.Adc.ScaleCalibration[0]);						
						result[i*DataStep+t] = ADC_INPUT_RANGES_E140[ADC_INPUT_RANGE]*(double)ReadBuffer[ReadBuffer_lastRead*DataStep+t]/8000.0;
						//result[i*DataStep+t] = (double)ReadBuffer[ReadBuffer_lastRead*DataStep+t];
						//result[i*DataStep+t] = 5.0;
					}

					/*
					//TEMP - ЭМУЛЯЦИЯ ЭНКОДЕРА
					//временная эмуляция
					//во второй канал суем синус
					double freq = encoderEmulationOutputVoltage  * 3 + 0.5;
					for (int i = 0; i < resultSize * DataStep - 3; i += 4)
					{						
						result[i + 1] = 5 * (1 + System::Math::Sin(freq * (emulationT += 0.001)));
					}*/
				}				
				return result;
			}
			else
			{
				int resultSize = 7;
				array<double>^ result = gcnew array<double>(resultSize*DataStep);
				for (int i = 0; i < resultSize * DataStep - 3; i += 4)
				{
					result[i + 0] = System::Math::Sin(emulationVoltage * (emulationT += 0.001));
					result[i + 1] = 0;
					result[i + 2] = s1_current;
					result[i + 3] = s2_current;
				}
				return result;
			}
		}			

		//Выходное напряжение на ЦАП
		property double outputVoltage
		{
			double get () 
			{
				if (!emulationMode)
					return (DacSample0/ModuleDescription.Dac.ScaleCalibration[0] - ModuleDescription.Dac.OffsetCalibration[0])*DAC_OUTPUT_RANGE_E140/2047.0;
				else
					return emulationVoltage;
			}			
			void set(double value)
			{
				encoderEmulationOutputVoltage = value;

				if (!emulationMode)
					SetVoltage(value);
				else
					emulationVoltage = value;
			}
		};

		//время между отсчетами
		property double deltaT
		{
			double get () 
			{
				if (!emulationMode)
					return (1/ap.KadrRate)/1000.0;
				else
					return (1/AdcRate * 2)/1000.0;
			}						
		};

		//установить значение первого и второго ттл
		bool SetTTL_1and2(bool s1, bool s2) //можно вызывать без изменений, само следит за этим
		{
			if (s1 != s1_current || s2 != s2_current)
			{
				bool sucesess;
				if (!emulationMode)				
					sucesess = SetTTL(s1, s2);				

				if (sucesess || emulationMode)
				{
					s1_current = s1;
					s2_current = s2;
					return true;
				}
				else
					return false;
			}
			else
				return false; //уже  и так всё было нормально
		}


	//конструктор и деструктор
		Module(bool emulation, int channelsQuantity, double adcRate, double inputRange, int dataStep)
		{	
			emulationMode = emulation;

			if (emulationMode)
			{				
				emulationVoltage = 0;
				emulationT = 0;
			}
			else
			{
				CHANNELS_QUANTITY = channelsQuantity;
				AdcRate = adcRate;
				DataStep = dataStep;  // кол-во получаемых отсчетов (кратное 32) для Ф. ReadData()

				if (inputRange < 156)
				{
					ADC_INPUT_RANGE = ADC_INPUT_RANGE_156mV_E140;
					inputRange = 156;
				}
				else if (inputRange < 625)
				{
					ADC_INPUT_RANGE = ADC_INPUT_RANGE_625mV_E140;
					inputRange = 625;
				}
				else if (inputRange < 2500)
				{
					ADC_INPUT_RANGE = ADC_INPUT_RANGE_2500mV_E140;
					inputRange = 2500;
				}
				else
				{
					ADC_INPUT_RANGE = ADC_INPUT_RANGE_10000mV_E140;
					inputRange = 10000;
				}

				InitializeModule();
				RunReading();	

				SetTTL(false, false);
			}

			
		}

		~Module()
		{
			if (!emulationMode)
			{
				//Выставим номер ошибки, соответсвующий ручному прерыванию потока сбора.
				ReadThreadErrorNumber = 0x5;
				//подождем пока поток сбора завершится
				Sleep(200);
			}
		}			
	};
}