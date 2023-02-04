
    $(function () {
        // sayfa yüklendikten sonrailk olarak data-note-id attribute'ne ship olan tüm elemetleri getirmem gerekecek. 
        var noteids = [];   // boş bir dizi tanımladım..

    // div olan ve data-note-id attribute'ü olan elementleri seciyorum "div[data-note-id]")
    // each fonksiyonu ile de her bir elementi geziyorum.  
    $("div[data-note-id]").each(function (i, e) {
        // push metodu ile elde ettiğim verileri noteids isimli diziye ekliyorum.($(e) ile elementi seçiyorum. data ile de not-id li veriye ulasmıs oluyorum.
        noteids.push($(e).data("note-id"));
            /*  console.log(noteids);*/
        });
    $.ajax({
        method: "POST",
    url: "/Note/GetLiked",
    data: {ids: noteids }
        }).done(function (data) {    //Actionda geriye bir veri gelmesı gerekiyor. gelen veri sistme logın olan userın begendiği notların listesi olaacak. dat ile bu listeeyi buradan alacagım.
            if (data.result != null && data.result.length > 0) {
                for (var i = 0; i < data.result.length; i++) {
                    var id = data.result[i];   //begenmiş oldugum notun id'sini almış oldum.
    var likedNote = $("div[data-note-id=" + id + "]");


    var btn = likedNote.find("button[data-liked]");
    var span = btn.find("span.like-heart");

    btn.data("liked", true);
    span.removeClass("glyphicon-heart-empty");
    span.addClass("glyphicon-heart");
                }

            }


        }).fail(function () { });

    // begenilmemiş bir gönderi oldugunda ve biz beğeni butonuna tıkladıgımızda 1- Veritabanında Likes tablosuna ilgili kayoıt girilmeli. 
    // Beğenilen bir gönderi için aynı butona tıklanıldıgında ilgili kayıt veritabanında silinmeli.
    // sayfa yüklendikten sonra data-liked attribute'ü olan butonlardan hangisine tıklandıysa click(). Aşagıdaki metot bunun için çalısacak.

    $("button[data-liked]").click(function () {
            // data-liked true ise gönderi beğenilmiş. false ise gönderi beğenilmemiştir.
            // önce butonu buluyırum.
            var btn = $(this); // o anki butonu btn değişkenine atıyorum.
    var liked = btn.data("liked"); // true ya da false değerini alıyorum.
    var noteid = btn.data("note-id"); //gönderi beğenildiyse beğenilmedi yaplacak.. begenilmediysebeğenildi  yapılacak.

    //ikonları ve like sayısını değiştireceğim spanları buluyorum.
    var spanHeart = btn.find("span.like-heart");
    var spanCount = btn.find("span.like-count");

    // Gerekli bilgileri sayfadan toparladım..
    $.ajax({
        method: "POST",
    url: "/Note/SetNoteLike",
    data: {"noteid": noteid, "liked": !liked }   // noteid'yi ve liked'in olması gereken değerini actıona gönderiyorum.
            }).done(function (data) {   // Actiondan gelen sonucu data ile alıyorum. 
                if (data.hasError) {    //datanin içindeki HasError'e göre ; true ise hata var kullanıcı uyarılıyor. 
        alert(data.errorMessage);
                }
    else {                     // false ise  sayfadaki liked değerini değiştiriyorum.
        liked = !liked
                    btn.data("liked", liked);

    spanCount.text(data.result);          // Action'dan gelen beğeni sayısını ilgili yere yazdırıyorum.

    // Beğeni butonundaki ikonları/class'ları kaldırıyorum
    spanHeart.removeClass("glyphicon-heart-empty");

    spanHeart.removeClass("glyphicon-heart");
    // Aşagıdakli if blogunda Beğenilme ya da beğenilmeyi kaldırma işlemine göre ikonu değiştiriyorum.
    if (liked) {
        spanHeart.addClass("glyphicon-heart");
                    } else {
        spanHeart.addClass("glyphicon-heart-empty");
                    }
                }



            }).fail(function () {
        // griş yapılmadıgında fakat begeni yapıldıgında aşagıdaki uyarıyı kullanıcıya gösteriyorum.
        alert("Gönderiyi beğenmek için sisteme giriş yapmalısınız. ");
            });

        })


    });



