<template>
  <div id="editor-container">
    <div id="toolbar">
      <button @click="format('bold')">Bold</button>
      <button @click="format('italic')">Italic</button>
      <button @click="format('underline')">Underline</button>
      <button @click="format('insertOrderedList')">Ordered List</button>
      <button @click="format('insertUnorderedList')">Unordered List</button>
      <button @click="format('justifyLeft')">Align Left</button>
      <button @click="format('justifyCenter')">Align Center</button>
      <button @click="format('justifyRight')">Align Right</button>
    </div>
    <div id="editor" contenteditable="true">
    </div>
    <div> sashdkajh </div>
  </div>
</template>
<style>
/* styles.css */
#editor-container {
  border: 1px solid #ddd;
  border-radius: 5px;
  max-width: 800px;
  margin: 0 auto;
  padding: 10px;
}

#toolbar {
  border-bottom: 1px solid #ddd;
  margin-bottom: 10px;
}

#toolbar button {
  margin-right: 5px;
  padding: 5px;
}

#editor {
  min-height: 300px;
  outline: none;
}
</style>
<script>
export default {
  name: "NoteEditor",
  methods: {
    async format(commond) { //加粗
      const s = window.getSelection();
      const selectedRange = s.rangeCount > 0 ? s.getRangeAt(0) : null;
      const myDiv  = document.getElementById("editor");
      if (selectedRange && myDiv.contains(selectedRange.commonAncestorContainer)) {
        if (s.rangeCount > 0) {
          const range = s.getRangeAt(0);
          if (commond == "bold") {
            const boldnode = document.createElement("strong");
            range.surroundContents(boldnode);
          } else if (commond == "italic") {
            const boldnode = document.createElement("i");
            range.surroundContents(boldnode);
          }
        }
      }
    }
  }
}
</script>