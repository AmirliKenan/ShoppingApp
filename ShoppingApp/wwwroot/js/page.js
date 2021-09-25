

    <script>


        function Delete(url)
        {

            swal({
                title: "Are you sure you want to Delete?",
                text: "You will not be able to restore the data!",
                icon: "warning",
                buttons: true,
                dangerMode: true
            }).then((willdelete) => {
                if (willdelete) {
                    $.ajax(
                        {
                            type: "DELETE",
                            url: url,
                            success: function (data) {
                                if (data.success) {
                                    toastr.success(data.message);
                                   

                                }
                                else {
                                    toastr.error(data.message);
                                }
                            }

                        });

                }
            });
        
    }

    </script>


