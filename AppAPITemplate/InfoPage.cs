using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace AppAPITemplate
{
	public class InfoPage : ContentPage
	{
		/*
			Displays all the information for the API menus	
		*/
		static Label One = new Label
		{
			Text = "",
			HorizontalOptions = LayoutOptions.Start,
		};
		static Label Two = new Label
		{
			Text = "",
			HorizontalOptions = LayoutOptions.Start,
		};
		static Label Three = new Label
		{
			Text = "",
			HorizontalOptions = LayoutOptions.Start,
		};
		static Label Four = new Label
		{
			Text = "",
			HorizontalOptions = LayoutOptions.Start,
		};
		static Label Five = new Label
		{
			Text = "",
			HorizontalOptions = LayoutOptions.Start,
		};



		StackLayout pageInfo = new StackLayout
		{
			Spacing = 0,
			VerticalOptions = LayoutOptions.FillAndExpand,

			Children =
			{
				One,
				Two,
				Three,
				Four,
				Five

			},
		};


		public MenuItem currentItem;

		//This holds the Views (it will switch between saying "Loading" and showing the info)
		public InfoPage(MenuItem item)
		{
			currentItem = item;
			Content = pageInfo;
		}


		protected override async void OnAppearing()
		{
			base.OnAppearing();

			MenuItem item = new MenuItem
			{
				Name = "YHOO",
				Description = "Yahoo Finance"
			};

			List<string> list = await CallAPI(item);
			One.Text = list[0];
			Two.Text = list[1];
			Three.Text = list[2];
			Four.Text = list[3];
			Five.Text = list[4];
		}

		static async Task<List<string>> CallAPI(MenuItem menuItem)
		{
			string response = await GetResponseFromAPI(menuItem);

			List<string> list = ConstructList(response);

			return list;
		}

		static async Task<string> GetResponseFromAPI(MenuItem menuItem)
		{
			string query = ConstructQuery(menuItem);

			//string results = "[{ One : \"Thomas 2\", Two : \"Is the best 2\", Three : \"And number three \", Four : \"And is four too much\", Five : \"A a high five\"}]"; //string results = call query.
			using (var client = new HttpClient())
			{
				var response = await client.GetStringAsync(query);
				return response.ToString();
			}
		}

		static string ConstructQuery(MenuItem menuItem)
		{
			string query = "https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quote%20where%20symbol%20in%20(%22" + menuItem.Name + "%22)&format=json&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=";

			return query;
		}

		static List<string> ConstructList(string response)
		{

			List<string> items = new List<string>();

			dynamic jsonResult = JsonConvert.DeserializeObject(response); //var jsonResult = Newtonsoft.Json.Linq.JObject.Parse(results);


			string symbol = jsonResult["query"]["results"]["quote"]["symbol"].Value;
			string name = jsonResult["query"]["results"]["quote"]["Name"].Value;
			string daysLow = jsonResult["query"]["results"]["quote"]["DaysLow"].Value;
			string daysHigh = jsonResult["query"]["results"]["quote"]["DaysHigh"].Value;
			string time = jsonResult["query"]["created"].Value.ToString();

			items.Add(time);
			items.Add(name);
			items.Add(symbol);
			items.Add(daysLow);
			items.Add(daysHigh);

			return items;

		}

	
	}
}

/*

public class TimePage : ContentPage
	{
		readonly Label timeLabel = new Label
		{
			Text = "Loading...",
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			VerticalOptions = LayoutOptions.CenterAndExpand,
		};

		public TimePage()
		{
			Content = timeLabel;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			timeLabel.Text = await RequestTimeAsync();
		}

		static async Task<string> RequestTimeAsync()
		{
			using (var client = new HttpClient())
			{
				var jsonString = await client.GetStringAsync("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quote%20where%20symbol%20in%20(%22YHOO%22)&format=json&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=");
				var jsonObject = JObject.Parse(jsonString);
				return jsonObject["time"].Value<string>();
			}
		}
	}



*/
