using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace IoTControl.UI
{
	public class InputControlWlegend : UserControl
	{
		private TextBlock Legend;
		private TextBox Input;
		private string legend = "?";
		public string Value {get=>Input.Text; set=>Input.Text=value.ToString();} //int.Parse(Input.Text)
		public InputControlWlegend() { }
		public InputControlWlegend(string legend) {
			this.legend = legend;
		}
		protected override void OnInitialized(EventArgs e)
		{
			base.OnInitialized(e);
			Width = 70;
			Height = 40;
			Border border = new Border()
			{
				Style = (Style)this.FindResource("BarderUI")
			};
			StackPanel panel = new StackPanel() {Margin = new Thickness(5)};
			panel.Orientation = Orientation.Horizontal;
			Legend = new TextBlock() { Text = legend, VerticalAlignment = VerticalAlignment.Center, Width = this.Width * 0.2 };
			panel.Children.Add(Legend);
			Input = new TextBox() { VerticalAlignment = VerticalAlignment.Center, Width = this.Width * 0.5, Text = "0"};
			panel.Children.Add(Input);
			border.Child = panel;
			Content = border;
		}
	}
}
