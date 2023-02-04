
    modelComponentBodyId = '#modal_yorum_body';
    var noteId = -1;

    $(function () {

        $('#modal_yorum').on('show.bs.modal', function (e) {
            var btn = $(e.relatedTarget);
            noteId = btn.data("note-id");
            $('#modal_yorum_body').load('/Comment/ShowNoteComments/' + noteId)
        })

    });


    function doComment(btn, eventMode, commentId, spanId) {
        var button = $(btn);   // parametreden gelen btn nesnesini JQuery'de kullanıma hazırladım.

    var mode = button.data("edit-mode"); //partial'daki button içindeki data-edit-mode özelliğinin değerini alıyorum.

    if (eventMode === "edit-clicked") {
            if (!mode) {       // mode false ise aşagıdaki işlemleri yapacak
        button.data("edit-mode", true);   // data-edit-mode=true yapıyorum. Burası güncellenebilir bilgisini tutmak için kullanılır.
    button.removeClass("btn-warning");  // button'un clasında olan btn-warning class'ını kaldırıyorum
    button.addClass("btn-success");  // button'un clasında olan btn-success class'ını ekliyorum.

    var btnSpan = button.find("span");   // button'un içindeki span'ı bulup btnSpan isimli değişkene aktarıyorum.
    btnSpan.removeClass("glyphicon-edit"); //spanın içindeki edit class'ını kaldırıyorum.
    btnSpan.addClass("glyphicon-ok");  // span'ın class'ına glyphicon-ok class'ını ekliyorum.


    // doComment fonksiyonuna parametre olarak gönderilen spanId'si partial içindeki yorumun yazıldıgı span..comment.text'in yazıldıgı satırı ait olan span
    $(spanId).attr("contenteditable", true);
    // ilgili span'ın contenteditable özelliğine true değerini aktarıyoryum. böylece bu span edit edilebilir textbox'a dönüşecek.
    $(spanId).addClass("editable");
    // ilgili span'a editable isimli bir class ekliyorum. Css ekleyip bir takım özelliklerini değiştireceğim.
    $(spanId).focus();  // span edit edilebilir hale geldikten sonra yanı textbox olduktan sonra cursor burada konumlansın diye yazdıgım satır.

            }
    else {
        // mode true oldugunda else kısmı çalısacak ve yukarıdaki işlemlerin tam tersi yapılacaak.
        button.data("edit-mode", false);
    button.removeClass("btn-success");
    button.addClass("btn-warning");

    var btnSpan = button.find("span");
    btnSpan.removeClass("glyphicon-ok");
    btnSpan.addClass("glyphicon-edit");

    $(spanId).attr("contenteditable", false);
    $(spanId).removeClass("editable");

    // Burada edit edilen yorumu veritabanına kaydetmemiz gereken kodları yazacagız. değişikliği ilgili controller ve actiona göndereceğiz.
    var txt = $(spanId).text();     // ilgili span içinden text değerini yani değiştirilmiş yorumu alıyoruz.
    $.ajax({
        method: "POST",
    url: "/Comment/Edit/" + commentId,
    data: {text: txt }
                }).done(function (data) {   // ajax metodu sonucu başarılı olursa burası yani done kısmı çalışacak
                    if (data.result) {

        $(modelComponentBodyId).load('/Comment/ShowNoteComments/' + noteId);

                    } else {
        // güncelleme yapılamaış oluyor.. burada da kullanıcıya mesaj vereceğiz.
        alert("Yorum güncellenemedi.");
                    }


                }).fail(function () {   // ajax metodu sonucu başarısız olursa burası yani fail kısmı çalışacak
        // Action ile ilgili bir problem olduğunda çalışacak bölüm.
        alert("Sunucuya bağlanamadı..");

                });

            }
                  
            }


    else if (eventMode === 'delete-clicked') {
            // Kullanıcı silme butonuna bastığında bir onay almamız gerekecek.
            var dialogResult = confirm("Yorum silinsin mi?");
    if (!dialogResult) return false;        // silme işlemi iptal edilecek.

    $.ajax({
        method: "GET",
    url: "/Comment/Delete/" + commentId
              
            }).done(function (data) {   // ajax metodu sonucu başarılı olursa burası yani done kısmı çalışacak
                // data ile action bir deger gelecek true ya false true: silme işlemi başarılı false:basarısız

                if (data) {
        // başarılı işe yorumlar sayfası güncellenecek.
        $(modelComponentBodyId).load('/Comment/ShowNoteComments/' + noteId);  //yourmlar bölümü tekrardan yüklenecek.
                }
    else {
        alert("Yorum silinemedi");
                }



            }).fail(function () {   // ajax metodu sonucu başarısız olursa burası yani fail kısmı çalışacak
        // Action ile ilgili bir problem olduğunda çalışacak bölüm.
        alert("Sunucuya bağlanamadı. Silme işlemi iptal edildi");

            });

        }
    else if (eventMode === 'new-clicked') {
            // eklenen yorumu ilgili input'un textinden alacagım ve ınsert Actıon'ına göndermem gerekecek. Veritabanına kayıt olsun diye..

            var txt = $('#new_comment_text').val(); //input içindeki veriyi almak için yazdıgımız kod
    //alert("Girilen yorum " + txt);

    $.ajax({
        method: "POST",
    url: "/Comment/Create/",
    data: {"Text": txt, "noteid": noteId }// "Text" çift tırnak içindekiler degişken adı

            }).done(function (data) {   // ajax metodu sonucu başarılı olursa burası yani done kısmı çalışacak
                // data ile action bir deger gelecek true ya false true: silme işlemi başarılı false:basarısız

                if (data) {
        // b_PartialComment ile verileri takrardan yüklüyoruz.
        $(modelComponentBodyId).load('/Comment/ShowNoteComments/' + noteId);  //yourmlar bölümü tekrardan yüklenecek.
                }
    else {
        alert("Yorum eklenemedi");
                }



            }).fail(function () {   // ajax metodu sonucu başarısız olursa burası yani fail kısmı çalışacak
        // Action ile ilgili bir problem olduğunda çalışacak bölüm.
        alert("Sunucuya bağlanamadı. Yorum ekleme işlemi iptal edildi");

            });


        }


            
        
    }

