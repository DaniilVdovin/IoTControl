﻿using System;
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

		public static async Task<Dictionary<string, string>> Connect(System.Net.Sockets.UdpReceiveResult dataFromRobots, IoT things)
		{

			string strData = Encoding.UTF8.GetString(dataFromRobots.Buffer);

			string[] subs = strData.Replace("\n", "").Split('#');
			var data = new Dictionary<string, string>();
			switch (things.type)
			{
				case "P":
				case "P1": //роботы. Отличаются наличием N, в роботах с 1 он есть (сейчас это не актуально, потому что новая прошивка сделала всех роботов типом без 1)
					data = things.GetRobotsData(subs); break;
				case "M":
				case "M1":
					data = things.GetRobotsData(subs); break;
				case "R":
				case "R1":
				case "R2": //терминалы. Они чем-то отличаются, но нигде не сказано чем и как
					data = things.GetTerminalData(subs); break;
				case "C": //камера
					data = things.GetCameraData(subs); break;
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
			return JsonSerializer.Deserialize<Dictionary<string, int>>(responseContent).ToDictionary(pair => pair.Key, pair => pair.Value.ToString());
		}

	}

}

