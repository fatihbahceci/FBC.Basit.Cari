////function jump(h) {
////    var url = location.href;               //Save down the URL without hash.
////    location.href = "#" + h;                 //Go to the target element.
////    history.replaceState(null, null, url);   //Don't like hashes. Changing it back.
////}

//For IE Support

//function jump(h) {
//    var top = document.getElementById(h).offsetTop; //Getting Y of target element
//    window.scrollTo(0, top);                        //Go there directly or some transition
//}


function jump(id) {
    const element = document.getElementById(id);
    if (element instanceof HTMLElement) {
        element.scrollIntoView({
            behavior: "smooth",
            block: "start",
            inline: "nearest"
        });
    }
}

console.log("Melabaaaa");
createBrowserId();
function getCookie(name) {
    var dc = document.cookie;
    var prefix = name + "=";
    var begin = dc.indexOf("; " + prefix);
    if (begin == -1) {
        begin = dc.indexOf(prefix);
        if (begin != 0) return null;
    }
    else {
        begin += 2;
        var end = document.cookie.indexOf(";", begin);
        if (end == -1) {
            end = dc.length;
        }
    }
    // because unescape has been deprecated, replaced with decodeURI
    //return unescape(dc.substring(begin + prefix.length, end));
    return decodeURI(dc.substring(begin + prefix.length, end));
}

/**
 * 
 * ;max-age=max-age-in-seconds (e.g., 60*60*24*365 or 31536000 for a year)
   ;expires=date-in-GMTString-format If neither expires nor max-age specified it will expire at the end of session.
 */



//function getCookieValue(key) {
//    return document.cookie
//        .split('; ')
//        .find(row => row.startsWith(key + '='))
//        .split('=')[1];
//    //if (document.cookie.split(';').some((item) => item.trim().startsWith(key + '='))) {

//    //}
//}

function uuidv4() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

function createBrowserId() {
    var myCookie = getCookie("fbc-bw-id");

    if (myCookie == null) {
        var uid = uuidv4();
        document.cookie = "fbc-bw-id=" + uid;
        console.log("Yoktur: " + uid)
    }
    else {
        console.log("Vardir: " + myCookie);
    }
}

  window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
  }
