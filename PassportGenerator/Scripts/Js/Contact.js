//function btnClick() {

//}


// $(document).ready(function () {
//     console.log("Document ready!");
//     $('#button').click(function () {
//         var name = $('#name').val();
//         var email = $('#email').val();

//         var obj = {
//             Name: name,
//             Email: email
//         }

//         SubmitForm(obj);


//     });
//        });


//function SubmitForm(obj) {
//    $.ajax({
//        url: "/Home/AddStudent",
//        method: "POST",
//        data: obj,
//        success: function (data) {
//            console.log(data);
//        },
//        error: function (err) {
//            console.log(err);
//            console.log("Failed");
//        }
//    });
//}