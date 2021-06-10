/** 
* @class g_DataList tool
* @constructor
* @return g_DataList object
*/
var g_DataList = {
    /**
    * 傳送資料的Form的ID
    * @type String
    */
    strFormID: "#fmQuery"
    ,
    /**
    * List的資料來源
    * @type String
    */
    strUrl: "/Home/QueryDataList"
    ,
    /**
    * 清單(Table)的區塊ID
    * @type String
    */
    strListID: "#divDataList"
    ,
    /**
    * Init
    * @function
    * @author Cindy
    */
    Init: function () {
        var _that = g_DataList;

        //設定分頁
        g_Paging.strFormID = "#fmQuery";
        g_Paging.strUrl = "/Home/QueryDataPaging";
        g_Paging.strPagingID = "#divPagingList";
        g_Paging.Init();

        //設定資料
        _that.strFormID = "#fmQuery";
        _that.strUrl = "/Home/QueryDataList"
        _that.strListID = "#divDataList";


        //查詢
        $(document).on("click", "#btnQuery", function () {
            g_Paging.LoadPaging();
            _that.LoadList();
            return false;
        });

        //預設載入
        $("#btnQuery").click();

    },
    /*
    * @function 編輯資料
    * @param {String} Id 流水號
    */
    EditData: function (Id) {
        location.href = "/Home/SaveDataList?Id=" + Id;
    },
    /**
    * 載入資料
    * @function
    */
    LoadList: function () {
        var _that = g_DataList;
        $.ajax({
            url: _that.strUrl,
            type: 'POST',
            data: JSON.stringify($(_that.strFormID).serializeObject()),
            async: false,
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                alert(thrownError);
            },
            success: function (result) {
                //判斷是否為jason格式
                if (_that.IsJsonString(result)) {
                    var objResult = jQuery.parseJSON(result);
                    alert(objResult.ReturnMessage);
                }
                else {
                    $(_that.strListID).html(result);
                }
            }
        });
    },
    /*
    * @function 刪除資料
    */
    DelData: function (Id) {
        var _that = g_DataList;
        if (confirm("確定要刪除此筆資料?") == true) {
            $.ajax({
                url: "/Home/DelData",
                type: 'POST',
                data: "{strId:" + Id + "}",
                async: false,
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(thrownError);
                },
                success: function (result) {
                    if (result.Result) {
                        alert(result.ReturnMessage);
                        _that.LoadList();
                        g_Paging.LoadPaging();
                    }
                }
            });
        }
    },
    /**
   * 判斷是否為jason格式的資料
   * @function
   * @param {String} 參數名稱
   * @returns {String} 參數值
   */
    IsJsonString: function (value) {
        try {
            JSON.parse(value);
        } catch (e) {
            return false;
        }
        return true;
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

