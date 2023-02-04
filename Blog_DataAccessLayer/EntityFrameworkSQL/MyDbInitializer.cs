using Blog_Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_DataAccessLayer.EntityFrameworkSQL
{
	public class MyDbInitializer:CreateDatabaseIfNotExists<BlogContext>
	{
		// FakeData dll'line ekledikten sonra bu Class'ı oluşturuyorum.
		// Bu Class, Database ilk oluştugu anda ilk verilerin yani geliştirme aşamasında kullanacagımız test verilerin oluşturulup database'e yüklenmesi amacı ile oluşturulmustur.
		// bu class'ın ne zaman çalışacagına ilişkin bilgiyi miras yoluyla verdiğimiz farklı bir class belirleyecek.
		//createdatabaseIfnotexists <> :database yoksa çalısır.
		// DropCreateDatabaseAlways<BlogContext> :database sil ve yeniden yarat her çalıştıgında
		//DropCreateDatabaseIfModelChanges  : eğer herhangi bir tablo değişirse Database'i sil ve yeniden yarat


		protected override void Seed(BlogContext context)
		{
			// Öncelikle 2 tane kullanıcı yaratalım.
			BlogUser admin =new BlogUser()
			{
				Name="Admin",
				Surname="Admin",
				UserProfileImage = "user-profile.jpg",
				Email ="admin@admin.com",
				ActivateGuid=Guid.NewGuid(),
				IsActive=true,
				IsAdmin=true,
				Username="admin",
				Password="123456",
				CreatedDate=DateTime.Now,
				ModifiedDate=DateTime.Now,
				ModifiedUserName="admin",

			};

			BlogUser standartUser=new BlogUser()
			{
				Name = "Dilan",
				Surname = "Bulak",
				UserProfileImage = "user-profile.jpg",
				Email = "bulak@gmail.com",
				ActivateGuid = Guid.NewGuid(),
				IsActive = true,
				IsAdmin = true,
				Username = "bulakdilan",
				Password = "123456",
				CreatedDate = DateTime.Now,
				ModifiedDate = DateTime.Now.AddMinutes(5),
				ModifiedUserName = "bulakdilan",

			};
			context.BlogUsers.Add(admin);
			context.BlogUsers.Add(standartUser);

			for (int i = 0; i < 10; i++)
			{
				BlogUser user =new BlogUser()
				{
					Name = FakeData.NameData.GetFirstName(),
					Surname = FakeData.NameData.GetSurname(),
					UserProfileImage = "user-profile.jpg",
					Email = FakeData.NetworkData.GetEmail(),
					ActivateGuid = Guid.NewGuid(),
					IsActive = true,
					IsAdmin = true,
					Username = $"user-{i}",
					Password = "123",
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now.AddMinutes(5),
					ModifiedUserName = $"user-{i}"

				};
				context.BlogUsers.Add(user);
			}

			context.SaveChanges();

			// Kullanıcı listesini database'ten alıyorum. Note ve Commet gibi tabloarda da kullanacagım
			List<BlogUser> userList =context.BlogUsers.ToList();

			// Fake kategori eklenecek.
			for (int i = 0; i < 10; i++)
			{
				Category category = new Category()
				{
					Title =FakeData.PlaceData.GetCounty(),
					Description=FakeData.PlaceData.GetAddress(),
					CreatedDate = DateTime.Now,
					ModifiedDate = DateTime.Now.AddMinutes(5),
					ModifiedUserName ="bulakdilan"
				};

				context.Categories.Add(category);

				// FAKE NOTE EKLİYORUM.
				for (int j = 0; j < FakeData.NumberData.GetNumber(3,15); j++)
				{
					BlogUser user_note= userList[FakeData.NumberData.GetNumber(0, userList.Count -1)];
					Note note=new Note()
					{
						Title=FakeData.PlaceData.GetCity(),
						Text=FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1,4)),
						Category= category,
						IsDraft=false,
						LikeCount=FakeData.NumberData.GetNumber(1,12),
						CreatedDate =FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
						ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
						ModifiedUserName =user_note.Username,
						Owner = user_note
					};
					category.Notes.Add(note);

					// comment eklemek için

					for (int k = 0; k < FakeData.NumberData.GetNumber(5,15); k++)
					{
						BlogUser commentuser= userList[FakeData.NumberData.GetNumber(0, userList.Count - 1)];

						Comment comment = new Comment()
						{
							Text = FakeData.TextData.GetSentence(),
							Owner = commentuser,
							CreatedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
							ModifiedDate = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
							ModifiedUserName = commentuser.Username,

						};
						note.Comments.Add(comment);
					}

					//Fake Like datası ekliyorum.
					for (int m = 0; m < note.LikeCount; m++)
					{
						Liked liked =new Liked()
						{
							LikedUser =user_note,
						};
						note.Likes.Add(liked);
					}
				}
			}
			context.SaveChanges();
		}
	}
}
