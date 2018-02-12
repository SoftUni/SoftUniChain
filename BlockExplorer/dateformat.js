function formatDuring(mss) {
    var days = parseInt(mss / (1000 * 60 * 60 * 24));
    var hours = parseInt((mss % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutes = parseInt((mss % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = (mss % (1000 * 60)) / 1000;
    if (days == 1) {
        return "One day ago";
    }
    if (days == 2) {
        return "Two days ago";
    }
    if (days > 2) {
        return "A few days ago";
    }
    if (days == 0) {
        if (hours == 2) {
            return "Two hours ago";
        }
        if (hours == 1) {
            return "One hour ago";
        }
        if (hours > 2) {
            return "A few hours ago";
        }
        if (hours == 0) {
            if (minutes == 2) {
                return "Two minutes ago";
            }
            if (minutes == 1) {
                return "One minute ago";
            }
            if (minutes > 2) {
                return "A few minutes ago";
            }
            if (minutes == 0) {
                return "A few seconds ago";
            }
        }
    }
}
Date.prototype.format = function(fmt) {
    var o = {
        "M+" : this.getMonth()+1,                 //Month
        "d+" : this.getDate(),                    //Date
        "h+" : this.getHours(),                   //Hours
        "m+" : this.getMinutes(),                 //Minutes
        "s+" : this.getSeconds(),                 //Seconds
        "q+" : Math.floor((this.getMonth()+3)/3), //Quarterly
        "S"  : this.getMilliseconds()             //Milliseconds
    };
    if(/(y+)/.test(fmt)) {
        fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));
    }
    for(var k in o) {
        if(new RegExp("("+ k +")").test(fmt)){
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));
        }
    }
    return fmt;
}