using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using Newtonsoft.Json;

namespace AppAPITemplate
{
	public class FirstMenu : Menu
	{
		public FirstMenu()
		{

			Title = "API Template";
			Content = list;

			list.ItemsSource = CallAPI(); //Need to add an "await" into this

			list.ItemTapped += (sender, e) =>
			{
				list.SelectedItem = null;
				//Navigation.PushAsync(new SecondMenu(e.Item as MenuItem));
				ClickMenuItem(e.Item as MenuItem);
			};
		}

		public List<MenuItem> CallAPI()
		{
			List<MenuItem> menuList;

			List<string> myStringColumn = new List<string>();
			using (var fileReader = File.OpenText(inFile))
			using (var csvResult = new CsvHelper.CsvReader(fileReader))
			{
				while (csvResult.Read())
				{
					string stringField = csvResult.GetField<string>("Header Name");
					myStringColumn.Add(stringField);
				}
			}


			return menuList;
		}

		public void ClickMenuItem(MenuItem itemClicked)
		{
			//Implements menu item click
			//e.g. Navigation.PushAsync(new SecondMenu(e.Item as string));
			Navigation.PushAsync(new SecondMenu(itemClicked));
		}

	}
}

