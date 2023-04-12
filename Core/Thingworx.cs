using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Ink;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Text.Json.Nodes;
using System.Runtime.CompilerServices;

namespace IoTControl.Core
{
	internal class Thingworx
	{
		
		public static async Task<JsonObject> Connect(System.Net.Sockets.UdpReceiveResult dataFromRobots, IoT things)
		{
			// Примерный вид данных приходящих с робота - [M:6:0:32:2574:1756:2231:1152:641:550#T:6:0:32:44:43:46:43:48:0#L:6:0:32:2041:2226:2088:2070:2080:2048#]
			// Создаем объект, который будем отправлять на ThingWorx

			string strData = Encoding.UTF8.GetString(dataFromRobots.Buffer);

			string[] subs = strData.Replace("\n", "").Split('#');
			var data = new Dictionary<string, string>();
			switch (things.type)
			{
				case "P":
				case "P1": //роботы. Отличаются наличием N, в роботах с 1 он есть (сейчас это не актуально, потому что новая прошивка сделала всех роботов типом без 1)
					data = GetRobotsData(subs, data); break;
				case "M":
				case "M1":
					data = GetRobotsData(subs, data); break;
				case "R":
				case "R1":
				case "R2": //терминалы. Они чем-то отличаются, но нигде не сказано чем и как
					data = GetTerminalData(subs,data); break;
				case "C": //камера
					data = GetCameraData(subs, data); break;
				case "T": // не получает данные с устройства  TrafficLight
				case "B": // нужно делать самостоятельно(т.к такой вещи не существует, с оригинального IoT control center приходит огромный, бесполезный пакет данных). отправлять нужно "c" BarcodeReader
				case "B1": // такого у нас нет, исходя из логики, можно просто предположить, что там приходит примерно такой пакет [B:"d1":"d2":"d3"#], где ключи это значения               LightBarrier
				case "D": // то же самое(B1), но [D:"n":"s":"c"#] (не уверен), где n - lastcommandnumber, c - count, s - status.																 Dispenser
				case "L": // сервисный логический (мобильный) робот OMG ☆*: .｡. o(≧▽≦)o .｡.:*☆        В данном контексте - OMG=ЧТОЭТОЯНЕПОНИМАЮАЛЛО
				case "AR": // дополненная реальность OMG ᓚᘏᗢ
				case "CS": // составное модульное смарт-устройство OMG (❁´◡`❁)
					data = null; break;
				default:
					data = null; break;
			}

			string json = JsonSerializer.Serialize(data); // Преобразуем объект в JSON
			Debug.WriteLine(json);
												  // Создаем HttpClient и настраиваем запрос
			var client = new HttpClient();
			client.DefaultRequestHeaders.Add("appKey", "5d14dd56-4ac7-49da-904c-5837d5f73c08");
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			// Отправляем запрос и получаем ответ
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await client.PostAsync($"http://192.168.0.250:8080/Thingworx/Things/{things.name}/Services/{things.service}", content);
			var responseContent = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<JsonObject>(responseContent);
		}

		private static Dictionary<string, string> GetRobotsData(string[] Subs, Dictionary<string, string> Data) {

			Debug.WriteLine(Subs[0].Length);
				for (int i = 0; i < Subs.Length - 1; i++)
				{
					var sub = Subs[i].Split(':');
					for (int j = 4; j <= 9; j++)
					{
					try
						{
							Data.Add(sub[0].ToLower() + (j - 3), sub[j]);

						}
					catch
						{
							Data.Add(sub[0].ToLower() + (j - 3), "0");

						}
					}
				}

				Data.Add("c", Subs[1].Split(':')[1]); // видимо, с последним патчем прошивки робота он перестал приходить(или никогда не приходил (_　_)。゜zｚＺ )
				Data.Add("s", Subs[1].Split(':')[2]);
				Data.Add("n", Subs[2].Split(':')[3]);

			return Data;
			}
		private static Dictionary<string, string> GetTerminalData(string[] Subs, Dictionary<string, string> Data)
		{
			Data.Add("p", Subs[0].Split(':')[1]);
			Data.Add("b1", Subs[0].Split(':')[2]);
			Data.Add("b2", Subs[0].Split(':')[3]);
			Data.Add("b3", Subs[0].Split(':')[4]);

			return Data;
		}
		private static Dictionary<string, string> GetCameraData(string[] Subs, Dictionary<string, string> Data)
		{
			string[] strokes = Subs[0].Split(':');
			for (int i=1; i < strokes.Length; i++)
			{
				Data.Add(("l"+(i-1)), strokes[i]);
			}

			return Data;
		}
	}


	}

