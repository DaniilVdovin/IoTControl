using IoTControl.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IoTControl.Core
{
	/*	public static class ThingProperties
		{
			public  Dictionary<string, Dictionary<string, Dictionary<string, string>>> MyDictionary { get; set; }
			public  string cmd_pkgZero;

			public ThingProperties()
			{
				MyDictionary = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>
				{
					{
					"P", new Dictionary<string, Dictionary<string, string>>
					{
						{ "control", new Dictionary<string, string> {
							{"X","0"},
							{"Y","0"},
							{"Z","0"},
							{"V","0"}

						}
						},
						{ "monitoring", new Dictionary<string, string> {
							{"m1","0"},
							{"m2","0"},
							{"m3","0"},
							{"m4","0"},
							{"m5","0"},
							{"m6","0"},
							{"t1","0"},
							{"t2","0"},
							{"t3","0"},
							{"t4","0"},
							{"t5","0"},
							{"t6","0"},
							{"l1","0"},
							{"l2","0"},
							{"l3","0"},
							{"l4","0"},
							{"l5","0"},
							{"l6","0"},
							{"s","0"},
							{"c","0"},
							{"n","0"},
						}
						}
					}
				},
				{
					"M", new Dictionary<string, Dictionary<string, string>>
					{
						 { "control", new Dictionary<string, string> {
							{"X","0"},
							{"Y","0"},
							{"T","0"},
							{"Z","0"},
							{"V","0"}
						} },
						 { "monitoring", new Dictionary<string, string> {
							{"m1","0"},
							{"m2","0"},
							{"m3","0"},
							{"m4","0"},
							{"m5","0"},
							{"t1","0"},
							{"t2","0"},
							{"t3","0"},
							{"t4","0"},
							{"t5","0"},
							{"l1","0"},
							{"l2","0"},
							{"l3","0"},
							{"l4","0"},
							{"l5","0"},
							{"s","0"},
							{"c","0"},
							{"n","0"},
						} }
					}
				},
							{
					"R2", new Dictionary<string, Dictionary<string, string>>
					{
						 { "control", new Dictionary<string, string> {
							{"L1","0"},
							{"L2","0"},
							{"L3","0"},
							{"L4","0"},
							{"D1","0"},
							{"DT","hello"},
						} },
						 { "monitoring", new Dictionary<string, string> {
							{"p","0"},
							{"b1","0"},
							{"b2","0"},
							{"b3","0"},
						} }
					}
				},
										{
					"C", new Dictionary<string, Dictionary<string, string>>
					{
						 { "control", new Dictionary<string, string> {
							{"G","0"},
						} },
						 { "monitoring", new Dictionary<string, string> {
							{"l1","00000"},
							{"l2","00000"},
							{"l3","00000"},
							{"l4","00000"},
							{"l5","00000"},
							{"l6","00000"},
						} }
					}
				},
													{
					"T", new Dictionary<string, Dictionary<string, string>>
					{
						 { "control", new Dictionary<string, string> {
							{"L1","0"},
							{"L4","0"},
							{"L3","0"},
							{"L2","0"},
						} }
					}
				},

				};
			}
			public Dictionary<string, string> GetValue(string key1, string key2)
			{
				if (MyDictionary.TryGetValue(key1, out var dict1) &&
					dict1.TryGetValue(key2, out var dict2))
				{
					return dict2;
				}
				else
				{
					// Обработка исключения, когда ключ отсутствует в словаре
					return new Dictionary<string, string>
					{

					};
				}
			}
			public string getFirstLetter(string key1)
			{
				switch (key1)
				{
					case "P":
					case "P1": // роботы. Отправка отличается наличием N. у букв с цифрками есть N, но на новых прошивках в этом нет смысла 
						cmd_pkgZero = "p"; break;
					case "M":
					case "M1":
						cmd_pkgZero = "g"; break;
					case "R":
					case "R1":
					case "R2": // терминалы. Они чем-то отличаются, но нигде не сказано чем и как
						cmd_pkgZero = "r"; break;
					case "C": // камера
						cmd_pkgZero = "c"; break;
					case "T": // Лампы 
						cmd_pkgZero = "l"; break; //o(*￣▽￣*)ブ　 4 и 2 перепутаны. Можно попробовать сделать foreach для него или senddatato... 
					case "D": // Диспенсер																																						 Dispenser
						cmd_pkgZero = "p"; break;
					case "B": // на него отправлять ничего не нужно																																 BarcodeReader
					case "B1": // на него отправлять ничего не нужно																											                 LightBarrier
					case "L": // сервисный логический (мобильный) робот OMG ☆*: .｡. o(≧▽≦)o .｡.:*☆        В данном контексте - OMG=ЧТОЭТОЯНЕПОНИМАЮАЛЛО
					case "AR": // дополненная реальность OMG ᓚᘏᗢ
					case "CS": // составное модульное смарт-устройство OMG (❁´◡`❁) 
						break;
					default:
						break;

				}
				return cmd_pkgZero;

			}
		}*/
	public class NestedValues{
		public Dictionary<string, string> control {
			get;
			set;
		}
		public Dictionary<string, string> monitoring {
			get;
			set;
		}
		public string firstLetter { get; set; }

		public NestedValues(IEnumerable<string> c,IEnumerable<string> m, IEnumerable<string> a,IEnumerable<string> b,string fl)
		{
			this.firstLetter = fl;
			IEnumerable<string> temp = m.Select((value) => value);
			IEnumerable<string> stemp = c.Select((value) => value);
			if (a != null) foreach (var item in a) temp.Append(item);
			if (b != null)foreach (var item in b) stemp.Append(item);
			this.control = stemp.ToDictionary(x => x, x => "0");
			this.monitoring = temp.ToDictionary(x => x, x => "0");
		}
	}
	public static class robotProperties { 
		public static Dictionary<string, NestedValues> MyDictionary { get; set;}

		public static void InitProperty() {
			MyDictionary = new Dictionary<string, NestedValues>();
			var temp = Enumerable.Range(1, 6).Select(i => $"m{i}").Concat(Enumerable.Range(1, 6).Select(i => $"l{i}")).Concat(Enumerable.Range(1, 6).Select(i => $"t{i}"));

			MyDictionary.Add("P",
				new NestedValues(new string[] { "X", "Y", "Z", "V" },
				temp,
				new string[] { "s", "c", "n" },
				null,
				"p"));
			MyDictionary.Add("M",
				new NestedValues(new string[] { "X", "Y", "T", "Z", "V" },
				temp,
				new string[] { "s", "c", "n" },
				null,
				"g"));
			MyDictionary.Add("R2",
				new NestedValues(Enumerable.Range(1, 4).Select(i => $"L{i}"),
				Enumerable.Range(1, 3).Select(i => $"b{i}"),
				new string[] { "p" },
				new string[] { "D1", "DT" },
				"r"));
			MyDictionary.Add("C",
				new NestedValues(new string[] { "G" },
				Enumerable.Range(1, 6).Select(i => $"l{i}"),
				null,
				null,
				"c"));
			MyDictionary.Add("T",
				new NestedValues(new string[] { "L1", "L4", "L3", "L2" },
				new string[] { },
				null,
				null,
				"l"));
			MyDictionary.Add("B",
				new NestedValues(new string[] { },
				new string[] { },
				null,
				null,
				"zxc"));
		}

	}



}
