// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// تهيئة السلايدر
const swiper = new Swiper('.swiper-container', {
    loop: true, // تكرار الشرائح
    pagination: {
        el: '.swiper-pagination', // نقاط التصفح
        clickable: true,
    },
    navigation: {
        nextEl: '.swiper-button-next', // زر التالي
        prevEl: '.swiper-button-prev', // زر السابق
    },
    autoplay: {
        delay: 5000, // تغيير الشريحة كل 5 ثواني
    },
});