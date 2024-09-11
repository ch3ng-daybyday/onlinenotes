<template>
  <div class="note-editor">
    <!-- 笔记标题输入框 -->
    <input v-model="note.title" placeholder="标题" class="title-input" />  

    <!-- Quill 编辑器 -->
    <quillEditor v-model="note.content" :modules="quillModules"   @text-change="onEditorChange" class="quill-editor" />

    <!-- 保存按钮 -->
    <button @click="saveNote">Save Note</button>

    <!-- 提示信息 -->
    <p v-if="successMessage" class="success">{{ successMessage }}</p>
    <p v-if="errorMessage" class="error">{{ errorMessage }}</p>
  </div>
</template>

<script>
import { quillEditor } from 'vue3-quill';
import hljs  from 'highlight.js';
import 'highlight.js/styles/default.css';
import axios from 'axios';

export default {
  components: {
    quillEditor
  },
  data() {
    return {
      note: {
        title: '',
        content: ''
      },
      quillModules: {
        syntax: {
          highlight: text => hljs.highlightAuto(text).value,   
        },
        toolbar: [
          [{ header: [1, 2, false] }],
          ['bold', 'italic', 'underline'],
          [{ list: 'ordered' }, { list: 'bullet' }],
          ['link', 'image'],
          ['code-block'],
          [{ align: [] }],
          ['clean'],
        ]
      },
      successMessage: '',
      errorMessage: ''
    };
  },
  methods: {
    async saveNote() {
      try {
        const response = await axios.post('https://localhost:7235/api/notes/', this.note);
        if (response.status === 200) {
          this.successMessage = 'Note saved successfully!';
          this.note = { title: '', content: '' }; // 清空输入框
        }
      } catch (error) {
        this.errorMessage = 'Error saving the note. Please try again.';
        console.error('Error saving note:', error);
      }
    }
  }
};
</script>

<style scoped>
.note-editor {
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  max-width: 100%;
}

.title-input {
  font-size: 20px;
  font-weight: bold;
  margin-bottom: 20px;
  padding: 10px;
  width: 100%;
}

.quill-editor {
  margin-bottom: 20px;
  height: 100%;
}

button {
  padding: 10px 20px;
  background-color: #42b983;
  color: white;
  border: none;
  cursor: pointer;
}

button:hover {
  background-color: #369774;
}

.success {
  color: green;
}

.error {
  color: red;
}

.ql-toolbar {
  background-color: red
    /* Change toolbar background color */
}

.ql-toolbar .ql-formats {
  margin-right: 40 px;
  /* Adjust spacing between groups */
}

.ql-toolbar .ql-picker {
  font-size: 14px;
  /* Customize font size for dropdowns */
}

/* Custom button styles */
.ql-toolbar .ql-bold::before {
  content: 'B';
  /* Custom icon for bold */
  font-weight: bold;
}

.ql-toolbar .ql-italic::before {
  content: 'I';
  /* Custom icon for italic */
}
</style>