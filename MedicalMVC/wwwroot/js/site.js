// Function to set selected value in dropdown based on current path
document.addEventListener('DOMContentLoaded', function () {
    var currentPath = window.location.pathname.toLowerCase();
    var selectElement = document.getElementById('categorySelect');

    if (currentPath.includes('/product/alphabet')) {
        selectElement.value = 'AlphabetProd';
    } else if (currentPath.includes('/product/latest')) {
        selectElement.value = 'LatestProd';
    } else {
        selectElement.value = 'IndexProd';
    }

    selectElement.addEventListener('change', function () {
        var selectedValue = this.value;
        if (selectedValue === 'IndexProd') {
            window.location.href = '/product';
        } else if (selectedValue === 'AlphabetProd') {
            window.location.href = '/product/alphabet';
        } else if (selectedValue === 'LatestProd') {
            window.location.href = '/product/latest';
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const photosInput = document.getElementById('photos');
    const rightSection = document.querySelector('.right');

    photosInput.addEventListener('change', function () {
        const files = Array.from(this.files); // Convert FileList to Array

        // Clear previous file names
        rightSection.innerHTML = '';

        // Iterate through selected files and display their names
        files.forEach(file => {
            const fileName = file.name;
            const fileExtension = fileName.split('.').pop(); // Get file extension

            const fileIconClass = getFileIconClass(fileExtension); // Function to get icon class based on file extension

            // Create elements to display file name and icon
            const fileDiv = document.createElement('div');
            fileDiv.classList.add('mb-2', 'd-flex', 'align-items-center');

            const fileIcon = document.createElement('i');
            fileIcon.classList.add('fa-regular', fileIconClass);

            const fileNameSpan = document.createElement('span');
            fileNameSpan.textContent = fileName;

            // Append icon and file name to fileDiv
            fileDiv.appendChild(fileIcon);
            fileDiv.appendChild(fileNameSpan);

            // Append fileDiv to rightSection
            rightSection.appendChild(fileDiv);
        });
    });

    // Function to determine icon class based on file extension
    function getFileIconClass(extension) {
        switch (extension.toLowerCase()) {
            case 'pdf':
                return 'fa-file-pdf';
            case 'png':
            case 'jpg':
            case 'jpeg':
            case 'gif':
            case 'svg':
                return 'fa-file-image';
            default:
                return 'fa-file'; // Default icon class for unknown file types
        }
    }
});
document.addEventListener('DOMContentLoaded', function () {
    const thumbnails = document.querySelectorAll('.smallphoto img');
    const largeImage = document.getElementById('largeImage');

    // Function to set the initial large image
    function setInitialLargeImage() {
        if (thumbnails.length > 0) {
            const firstThumbnail = thumbnails[0];
            const firstThumbnailSrc = firstThumbnail.getAttribute('data-large');
            largeImage.src = firstThumbnailSrc;
            firstThumbnail.classList.add('activated'); // Corrected class name
        }
    }

    // Event listener for clicking on thumbnails
    thumbnails.forEach(thumbnail => {
        thumbnail.addEventListener('click', function () {
            // Remove 'activate' class from all thumbnails
            thumbnails.forEach(thumb => thumb.classList.remove('activated'));
            // Set 'activate' class on the clicked thumbnail
            this.classList.add('activated');
            // Update large image src to clicked thumbnail's data-large
            const thumbnailSrc = this.getAttribute('data-large');
            largeImage.src = thumbnailSrc;
        });
    });

    // Call the function to set initial large image
    setInitialLargeImage();
});


// Function to set active class for navigation links
document.addEventListener('DOMContentLoaded', () => {
    const windowPathname = window.location.pathname.toLowerCase();
    const windowSearch = window.location.search.toLowerCase(); // Include search part of the URL

    // Function to set active class for state links and category and nav
    function setActiveClassForStateLinks() {
        const stateLinkEls = document.querySelectorAll('.state-link');

        stateLinkEls.forEach(stateLinkEl => {
            const stateLinkPathname = new URL(stateLinkEl.href, window.location.origin).pathname.toLowerCase();
            const stateLinkSearch = new URL(stateLinkEl.href, window.location.origin).search.toLowerCase();

            if (windowPathname === stateLinkPathname && windowSearch === stateLinkSearch) {
                stateLinkEl.classList.add('activecategory');
            } else {
                stateLinkEl.classList.remove('activecategory');
            }
        });
    }

    // Function to set active class for category links
    function setActiveClassForCategoryLinks() {
        const categoryLinkEls = document.querySelectorAll('.category-link');

        categoryLinkEls.forEach(categoryLinkEl => {
            const categoryLinkPathname = new URL(categoryLinkEl.href, window.location.origin).pathname.toLowerCase();
            // Category links may not have search parameters, so we compare only pathname
            if (windowPathname === categoryLinkPathname || windowPathname.startsWith(categoryLinkPathname)) {
                categoryLinkEl.classList.add('activecategory');
            } else {
                categoryLinkEl.classList.remove('activecategory');
            }
        });
    }

    setActiveClassForStateLinks();
    setActiveClassForCategoryLinks();
});





//see selected photos

//see selected photos end

// Aziks code end


if (parseInt($('.left-items .cat-list').css('height')) == 240) {
    $('.left-items .list .show').show()
}
$('.left-items .list .show').click(function () {
    $('.left-items .cat-list').css('max-height', 'unset');
    $(this).hide()
    $('.left-items .list .hide').show()
});
$('.left-items .list .hide').click(function () {
    $('.left-items .cat-list').css('max-height', '240px');
    $(this).hide()
    $('.left-items .list .show').show()
});

//media filter
$('.filtr-btn').click(function () {
    $('.media-filter').show()
    $('body').css('overflow', 'hidden')
    if (parseInt($('.media-filter .cat-list').css('height')) == 240) {
        $('.media-filter .list .show').show()
    }
})
$('.media-filter .f-head .fa-xmark').click(function () {
    $('.media-filter').hide()
    $('body').css('overflow', 'scroll')
})
$('.media-filter .f-head button').click(function () {
    $('.media-filter').hide()
    $('body').css('overflow', 'scroll')
})
$('.media-filter .list .show').click(function () {
    $('.media-filter .cat-list').css('max-height', 'unset');
    $(this).hide()
    $('.media-filter .list .hide').show()
});
$('.media-filter .list .hide').click(function () {
    $('.media-filter .cat-list').css('max-height', '240px');
    $(this).hide()
    $('.media-filter .list .show').show()
});

$('.description .images .left .img').each(function () {
    $(this).click(function () {
        $('.description .images .left .img').removeClass('selected-img');
        $(this).addClass('selected-img');
        var newSrc = $(this).find('img').attr('src');
        $('.description .images .right img').attr('src', newSrc);
    });
});

$('.post  button').click(function () {
    $('.post .submit').css('display', 'block')
    $('body').css('overflow', 'hidden')
})

$('.post .submit a').click(function () {
    $('.post .submit').css('display', 'none')
    $('body').css('overflow', 'scroll')
})
$('.post .submit .fa-xmark').click(function () {
    $('.post .submit').css('display', 'none')
    $('body').css('overflow', 'scroll')
})

var brangSwiper = new Swiper(".description .media-left .mySwiper", {
    slidesPerView: 4,
    spaceBetween: 12,
    navigation: {
        nextEl: ".description .media-left .swiper-button-next",
        prevEl: ".description .media-left .swiper-button-prev",
    }
});
var swiper = new Swiper(".prd-box .mySwiper", {
    slidesPerView: 4,
    centeredSlides: true,
    spaceBetween: 20,
    loop: true,
    grabCursor: true,
    pagination: {
        el: ".swiper-pagination",
        clickable: true,
    },
});

$(".description .left .img").first().addClass("selected-img")
$(".description .right img").attr("src", $(".description .left .selected-img img").attr("src"));
$(".description .left .img").click(function () {
    $(".description .left img").removeClass("selected-img")
    $(this).addClass("selected-img")
    $(".description .right img").attr("src", $(".description .left .selected-img img").attr("src"));
})

//medina
$(".openMore").on("mouseover", function () {
    $(".more").css("height", "457px");
});
$(".openMore").on("mouseout", function () {
    $(".more").css("height", "0px");
});

$(".more").on("mouseover", function () {
    $(".more").css("height", "457px");
});
$(".more").on("mouseout", function () {
    $(".more").css("height", "0px");
});

$(".menu").on("click", function () {
    $(".otkr").css("display", "flex");
});

$(".fa-xmark").on("click", function () {
    $(".otkr").hide();
});


$(".lupa1").click(function () {
    $(".info3").hide();
    $(".search-group").addClass("active");
    $(this).hide();
})
$(".viyti").click(function () {
    $(".info3").show();
    $(".search-group").removeClass("active");
    $(".lupa1").show();
})
$(".otkr-search").click(function () {
    $(".search-group").addClass("active");
})
$(".otkr .bi-x-lg").click(function () {
    $(".search-group").removeClass("active");

})


$(".vibor_yazika").click(function () {
    $(".yaziki").toggleClass("yaziki2");

})




$(document).ready(function () {
    const dropdown = $('.dropdown');
    const dropdownContent = $('.dropdown-content');

    dropdown.hover(
        function () {
            dropdownContent.show();
        },
        function () {
            dropdownContent.hide();
        }
    );
});



