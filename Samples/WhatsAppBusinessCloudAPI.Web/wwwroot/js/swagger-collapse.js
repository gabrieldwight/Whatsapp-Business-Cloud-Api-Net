// Wait a second then collapse all Swagger accordions.
setTimeout(() => {
    var all = document.getElementsByClassName('expand-operation');
    for (var i = 0; i < all.length; i++) {
        all[i].click();
    }
}, 1000);
