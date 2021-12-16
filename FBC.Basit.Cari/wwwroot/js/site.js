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

