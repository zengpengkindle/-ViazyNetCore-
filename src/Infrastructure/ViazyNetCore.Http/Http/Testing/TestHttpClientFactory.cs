﻿//using System;
//using System.Net;
//using System.Net.Http;

//namespace ViazyNetCore.Http
//{
//	/// <summary>
//	/// IHttpClientFactory implementation used to fake and record calls in tests.
//	/// </summary>
//	public class TestHttpClientFactory : DefaultHttpClientFactory
//	{
//		/// <summary>
//		/// Creates an instance of FakeHttpMessageHander, which prevents actual HTTP calls from being made.
//		/// </summary>
//		/// <returns></returns>
//		public override HttpMessageHandler EasyMessageHandler(WebProxy proxy) {
//			return new FakeHttpMessageHandler(base.EasyMessageHandler());
//		}
//	}
//}