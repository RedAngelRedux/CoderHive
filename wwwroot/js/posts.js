let index = 0;

function AddTag() {

    // Get a reference to the TagEntry input element
    let tagEntry = document.getElementById("TagEntry");

    // Create a new Select Option
    let newOption = new Option(tagEntry.value, tagEntry.value);
    document.getElementById("TagList").options[index++] = newOption;

    // Clear out the TagEntry control
    tagEntry.value = "";
    return true;
}

function DeleteTag() {
    let tagCount = 1;

    while (tagCount > 0) {
        let selectedIndex = document.getElementById("TagList").selectedIndex;
        if (selectedIndex >= 0) {
            document.getElementById("TagList").options[selectedIndex] = null;
            --tagCount;
        }
        else {
            tagCount = 0;
        }
        index--;
    }
}

// This will select all of the entries in the TagList upon submit, to ensure they get passed to the Post Create
$("form").on("submit", function () {
    $("#TagList option").prop("selected", "selected");
})