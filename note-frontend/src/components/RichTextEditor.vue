<template>
  <div class="editor-container">
    <Toolbar
      class="editor-toolbar"
      :editor="editorRef"
      :defaultConfig="toolbarConfig"
      :mode="mode"
    />
    <Editor
      class="editor-content"
      v-model="valueHtml"
      :defaultConfig="editorConfig"
      :mode="mode"
      @onCreated="handleCreated"
      @onChange="handleChange"
 
      @onKeyup="handleKeyup"
    />
  </div>
</template>

<script setup>
import '@wangeditor/editor/dist/css/style.css'
import { Editor, Toolbar } from '@wangeditor/editor-for-vue'
import { ref, shallowRef, onBeforeUnmount,defineProps ,defineEmits} from 'vue'

const props = defineProps({
  modelValue: {
    type: String,
    default: ''
  },
  height: {
    type: String,
    default: '500px'
  },
  mode: {
    type: String,
    default: 'default'
  }
})

const emit = defineEmits(['update:modelValue', 'change'])

// 编辑器实例
const editorRef = shallowRef()

// 内容 HTML
const valueHtml = ref(props.modelValue)

// Markdown 语法映射
const markdownSyntax = {
  '#': { type: 'heading1', tag: 'h1' },
  '##': { type: 'heading2', tag: 'h2' },
  '###': { type: 'heading3', tag: 'h3' },
  '```': { type: 'code-block', tag: 'pre' },
  '>': { type: 'blockquote', tag: 'blockquote' },
  '*': { type: 'list-item', tag: 'li' },
  '-': { type: 'list-item', tag: 'li' },
  '**': { type: 'bold', tag: 'strong' },
  '_': { type: 'italic', tag: 'em' },
  '`': { type: 'code', tag: 'code' }
}

// 工具栏配置
const toolbarConfig = {
  excludeKeys: [],
  toolbarKeys: [
    'headerSelect',
    'blockquote',
    'bold',
    'italic',
    'underline',
    'through',
    'bulletedList',
    'numberedList',
    'todo',
    'code',
    'insertLink',
    'insertImage',
    'divider',
    'undo',
    'redo',
  ]
}

// 编辑器配置
const editorConfig = {
  placeholder: '请输入内容...\n支持 Markdown 快捷语法：\n# 标题1\n## 标题2\n### 标题3\n> 引用\n* 列表\n``` 代码块\n**粗体**\n_斜体_',
  autoFocus: false,
  MENU_CONF: {
    uploadImage: {
      server: '/api/upload/image',
      fieldName: 'file',
      maxFileSize: 10 * 1024 * 1024,
      maxNumberOfFiles: 10,
      allowedFileTypes: ['image/*'],
      metaWithUrl: true,
      customInsert(res, insertFn) {
        const url = res.data.url
        insertFn(url)
      },
    }
  }
}

// 键盘事件处理
// const handleKeydown = (e) => {
//   if (e.key === 'Tab') {
//     e.preventDefault() // 阻止默认的 Tab 行为
//   }
// }

const handleKeyup = (e, editor) => {
  // e.preventDefault();
  const text = editor.getText()
  const selection = editor.getSelection()
  if (!selection) return

  // 获取当前行的文本
  const lines = text.split('\n')
  const currentLineIndex = selection.top
  const currentLine = lines[currentLineIndex] || ''
  
  // 检查是否匹配 Markdown 语法
  Object.entries(markdownSyntax).forEach(([key, value]) => {
    if (currentLine.trim() === key) {
      // 删除当前行
      editor.deleteText({
        index: currentLineIndex,
        length: currentLine.length + 1
      })
      
      // 应用相应的格式
      switch (value.type) {
        case 'heading1':
          editor.cmd.do('heading', { level: 1 })
          break
        case 'heading2':
          editor.cmd.do('heading', { level: 2 })
          break
        case 'heading3':
          editor.cmd.do('heading', { level: 3 })
          break
        case 'code-block':
          editor.cmd.do('insertCode', '')
          break
        case 'blockquote':
          editor.cmd.do('blockquote')
          break
        case 'list-item':
          editor.cmd.do('insertUnorderedList')
          break
        case 'bold':
          editor.cmd.do('bold')
          break
        case 'italic':
          editor.cmd.do('italic')
          break
        case 'code':
          editor.cmd.do('code')
          break
      }
    }
  })
}

// 组件销毁时，也及时销毁编辑器
onBeforeUnmount(() => {
  const editor = editorRef.value
  if (editor == null) return
  editor.destroy()
})

// 编辑器回调函数
const handleCreated = (editor) => {
  editorRef.value = editor
}

const handleChange = (editor) => {
  emit('update:modelValue', editor.getHtml())
  emit('change', editor.getHtml())
}
</script>

<style scoped>
.editor-container {
  border: 1px solid #ccc;
  z-index: 100;
}

.editor-toolbar {
  border-bottom: 1px solid #ccc;
}

.editor-content {
  height: v-bind(height);
  overflow-y: hidden;
}

/* 添加代码块样式 */
:deep(.w-e-text-container pre) {
  background-color: #f6f8fa;
  border-radius: 3px;
  padding: 16px;
  overflow: auto;
  font-family: 'SFMono-Regular', Consolas, 'Liberation Mono', Menlo, Courier, monospace;
}

:deep(.w-e-text-container code) {
  background-color: rgba(175,184,193,0.2);
  border-radius: 3px;
  padding: 0.2em 0.4em;
  font-family: 'SFMono-Regular', Consolas, 'Liberation Mono', Menlo, Courier, monospace;
}

:deep(.w-e-text-container blockquote) {
  border-left: 4px solid #dfe2e5;
  padding: 0 1em;
  color: #6a737d;
}

/* 添加一些提示样式 */
.w-e-text-container[data-placeholder]::before {
  content: attr(data-placeholder);
  position: absolute;
  color: #999;
  pointer-events: none;
  white-space: pre-wrap;
}

.w-e-text-container.has-focus[data-placeholder]::before,
.w-e-text-container:not(:empty)[data-placeholder]::before {
  display: none;
}
</style> 