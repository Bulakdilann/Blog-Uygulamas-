using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_BussinesLayer
{
	public class BussinesLayerResult<T> where T:class
	{
		// HATA mesajlarını saklayan bir list tanımlıyorum.  //amaç hata mesajlarını kontrol etmek
		public List<string> Errors { get; set; }

		// eger hata mesajımız yok ise aşagıdaki nesneyi geriye döndüreceğiz.
		public T Result { get; set; }

		public BussinesLayerResult()
		{
			Errors =new List<string>();
		}
	}
}
