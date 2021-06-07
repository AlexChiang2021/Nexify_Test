/*
* @class g_Data tool
* @constructor
* @return g_Data object
*/
var g_Data = {
    /**
    * Init
    * @function
    */
    Init: function () {
        var _that = g_Data;

        $("#btnSave").on("click", function () {
            _that.SaveData();
        });
    },
    /*
    * @function 編輯資料
    */
    SaveData: function () {
        var _that = g_Data;
        $.ajax({
            url: "/Home/SaveDataList",
            type: 'POST',
            data: JSON.stringify($("form").serializeObject()),
            async: false,
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError);
            },
            success: function (result) {
                if (result.Result) {
                    alert(result.ReturnMessage);
                    location.href = "/Home/Index";
                }
                else {
                    alert(result.ReturnMessage);
                }
            }
        });
    }
}
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};


