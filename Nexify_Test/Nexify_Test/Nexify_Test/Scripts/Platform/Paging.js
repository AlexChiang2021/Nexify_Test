
/**
* @class 分頁
* @constructor
* @return g_Query object
*/
var g_Paging = {
    /* 
    * @function 分頁的區塊ID
    */
    strFormID: "#fmQuery"
    ,
    /**
    * 分頁的區塊ID
    * @type String
    */
    strPagingID: "#divPagingList"
    ,
    /**
    * 顯示 第n頁的ID
    */
    strPagingIndexID: "#sltPageIndex"
    ,
    /**
    * 每頁顯示n筆的ID
    */
    strPageSizeID: '#sltPageSize'
    ,
    /**
    * 顯示總筆數的ID
    */
    strTotalCountID: '#spanTotalCount'
    ,
    /* Init
    * @function List的資料來源
    */
    strUrl: "/Sample/QueryPaging"
    /**
    * @function 初始化
    */
    , Init: function () {
        var _that = g_Paging;

        //每頁顯示n筆
        $(document).on("change", _that.strPageSizeID, function () {

            var intPageSize = $(this).val();
            var intTotalCount = $(g_Paging.strTotalCountID)[0].innerText;
            var intPageCount = 1;
            if (intTotalCount > 0) {
                intPageCount = intTotalCount / intPageSize;
                if ((intTotalCount % intPageSize) > 0) {
                    intPageCount++;
                }
            } else {
                return false;//無總數不做任何動作
            }

            var sltPagingIndex = $(g_Paging.strPagingIndexID);
            //全部移除
            $(sltPagingIndex).find('option').remove();
            //新增選項
            for (var i = 1; i <= intPageCount; i++) {
                $(sltPagingIndex).append($("<option></option>").attr("value", i).text("第 " + i + " 頁"));
            }
            g_DataList.LoadList();
            return false;
        });

        //顯示每頁n筆有異動
        $(document).on("change", _that.strPagingIndexID, function () {
            g_DataList.LoadList();
            return false;
        });
    }
     ,
    /**
    * 載入資料
    * @function
    */
    LoadPaging: function () {
        var _that = g_Paging;
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
                if (_that.IsJsonString(result)) {
                    var objResult = jQuery.parseJSON(result);
                    alert(objResult.ReturnMessage);
                }
                else {
                    $(_that.strPagingID).html(result);
                }
            }
        });

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
