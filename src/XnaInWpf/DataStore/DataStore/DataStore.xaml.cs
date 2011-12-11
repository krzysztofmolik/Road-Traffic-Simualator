﻿//      *********    DO NOT MODIFY THIS FILE     *********
//      This file is regenerated by a design tool. Making
//      changes to this file can cause errors.
namespace Expression.Blend.DataStore.DataStore
{
	using System;
	using System.Collections.Generic;

	public class DataStoreGlobalStorage
	{
		public static DataStoreGlobalStorage Singleton;
		public bool Loading {get;set;}
		private List<WeakReference> registrar; 

		public DataStoreGlobalStorage()
		{
			this.registrar = new List<WeakReference>();
		}
		
		static DataStoreGlobalStorage()
		{
			Singleton = new DataStoreGlobalStorage();
		}

		public void Register(DataStore dataStore)
		{
			this.registrar.Add(new WeakReference(dataStore));
		}

		public void OnPropertyChanged(string property)
		{
			foreach (WeakReference entry in this.registrar)
			{
				if (!entry.IsAlive)
				{
					continue;
				}
				DataStore dataStore = (DataStore)entry.Target;
				dataStore.FirePropertyChanged(property);
			}
		}
		
		public bool AssignementAllowed
		{
			get
			{
				// Only assign from loading once
				if(this.Loading && this.registrar.Count > 0)
				{
					return false;
				}
				
				return true;
			}
		}

		private string _Name = string.Empty;

		public string Name
		{
			get
			{
				return this._Name;
			}

			set
			{
				if(!this.AssignementAllowed)
				{
					return;
				}
				
				if (this._Name != value)
				{
					this._Name = value;
					this.OnPropertyChanged("Name");
				}
			}
		}

		private bool _IsAddMode = false;

		public bool IsAddMode
		{
			get
			{
				return this._IsAddMode;
			}

			set
			{
				if(!this.AssignementAllowed)
				{
					return;
				}
				
				if (this._IsAddMode != value)
				{
					this._IsAddMode = value;
					this.OnPropertyChanged("IsAddMode");
				}
			}
		}

		private string _Property1 = string.Empty;

		public string Property1
		{
			get
			{
				return this._Property1;
			}

			set
			{
				if(!this.AssignementAllowed)
				{
					return;
				}
				
				if (this._Property1 != value)
				{
					this._Property1 = value;
					this.OnPropertyChanged("Property1");
				}
			}
		}
	}

	public class DataStore : System.ComponentModel.INotifyPropertyChanged
	{
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		
		public void FirePropertyChanged(string propertyName)
		{
			this.OnPropertyChanged(propertyName);
		}

		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}

		public DataStore()
		{
			try
			{
				System.Uri resourceUri = new System.Uri("/RoadTrafficConstructor;component/DataStore/DataStore/DataStore.xaml", System.UriKind.Relative);
				if (System.Windows.Application.GetResourceStream(resourceUri) != null)
				{
					DataStoreGlobalStorage.Singleton.Loading = true;
					System.Windows.Application.LoadComponent(this, resourceUri);
					DataStoreGlobalStorage.Singleton.Loading = false;
					DataStoreGlobalStorage.Singleton.Register(this);
				}
			}
			catch (System.Exception)
			{
			}
		}

		private string _Name = string.Empty;

		public string Name
		{
			get
			{
				return DataStoreGlobalStorage.Singleton.Name;
			}

			set
			{
				DataStoreGlobalStorage.Singleton.Name = value;
			}
		}

		private bool _IsAddMode = false;

		public bool IsAddMode
		{
			get
			{
				return DataStoreGlobalStorage.Singleton.IsAddMode;
			}

			set
			{
				DataStoreGlobalStorage.Singleton.IsAddMode = value;
			}
		}

		private string _Property1 = string.Empty;

		public string Property1
		{
			get
			{
				return DataStoreGlobalStorage.Singleton.Property1;
			}

			set
			{
				DataStoreGlobalStorage.Singleton.Property1 = value;
			}
		}
	}
}
