
namespace xPlug.BusinessService
{
	/// Author:	 Oluwaseyi Adegboyega - sadegboyega@xplugng.com
	/// Copyright © 2013. xPlug Technologies Limited. All Rights Reserved
	///*******************************************************************************
	///* Date Created: 	04-06-2013
	///* Date Generated:	25-11-2013 09:26:16
	///*******************************************************************************


	public class ServiceProvider
	{
		 private IServices _iServices = null;
		 private static ServiceProvider _newInstance;
		public ServiceProvider()
		{
			 _iServices = new StandardServices();
		}

		 public static IServices Instance()
		{
			 if(_newInstance == null)
			 {
				 _newInstance = new ServiceProvider();
			 }
			 return _newInstance._iServices;
		}

		public static ServiceProvider GetServiceProvider()
		{
			if(_newInstance == null)
			{
				 _newInstance = new ServiceProvider();
			}
			return _newInstance;
		}
	}


	//Class File Generated from Code<->Stripper 1.2.0.0 | All Rights Reserved
}
