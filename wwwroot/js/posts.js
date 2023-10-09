let index = 0;

function AddTag() {

    // Get a reference to the TagEntry input element
    let tagEntry = document.getElementById("TagEntry");

    // Use the Search function to detect an error state
    let searchResult = Search(tagEntry.value);
    if (searchResult != null) {

        // Trigger Sweet Alert
    //    Swal.fire({
    //        title: 'Error!',
    //        text: `${searchResult}`,
    //        icon: 'error',
    //        confirmButtonText: 'Cool'
    //    })
        swalWithDarkButton.fire({
            html: `<span class='font-weight-bolder'>${searchResult.toUpperCase()}</span>`
        })
    }
    else {
        // Create a new Select Option
        let newOption = new Option(tagEntry.value, tagEntry.value);
        document.getElementById("TagList").options[index++] = newOption;
    }

    // Clear out the TagEntry control
    tagEntry.value = "";
    return true;
}

function DeleteTag() {

    let tagCount = 1;
    let tagList = document.getElementById("TagList");
    if (!tagList) return false;

    if (tagList.selectedIndex == -1) {
        swalWithDarkButton.fire({
            html: "<span class='font-weight-bolder'>CHOOSE A TAG BEFORE DELETING</span>"
        });
        return true;
    }

    while (tagCount > 0) {
        if (tagList.selectedIndex >= 0) {
            tagList.options[tagList.selectedIndex] = null;
            --tagCount;
        }
        else {
            tagCount = 0;
        }
        index--;
    }
}

// The Search function will detect either an empty or a duplicate Tag
// and return an error string if an error is detected
function Search(str) {

    if (str == "") return 'Empty tags are not permitted';

    let tagList = document.getElementById("TagList");
    if (tagList) {
        let options = tagList.options;
        for (let i = 0; i < options.length; i++) {
            if (options[i].value == str) return `The tag #${str} is a duplicate`;
        }
    }
}

// This will select all of the entries in the TagList upon submit, to ensure they get passed to the Post Create
$("form").on("submit", function () {
    $("#TagList option").prop("selected", "selected");
})

// Look for the tagValues variable to see if it has data
if (tagValues != "") {
    let tagArray = tagValues.split(",");
    for (let loop = 0; loop < tagArray.length; loop++) {
        // Load up or replace the options that we have
        ReplaceTag(tagArray[loop], loop);
        index++
    }
}

function ReplaceTag(tag, index) {
    let newOption = new Option(tag, tag);
    document.getElementById("TagList").options[index] = newOption;
}

const swalWithDarkButton = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-danger btn-sm btn-outline-dark'
    },
    imageUrl: '/images/RedX.png',
    timer: 3000,
    buttonsStyling: false
})
