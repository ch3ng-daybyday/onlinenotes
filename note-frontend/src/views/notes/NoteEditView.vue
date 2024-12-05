<template>
  <div class="note-edit-container">
    <div class="note-header">
      <input
        type="text"
        v-model="noteTitle"
        class="note-title"
        placeholder="请输入标题"
      />
      <div class="note-actions">
        <button 
          class="save-btn"
          :disabled="saving"
          @click="handleSave"
        >
          {{ saving ? '保存中...' : '保存' }}
        </button>
      </div>
    </div>
    
    <div class="note-content">
      <RichTextEditor
        v-model="noteContent"
        height="calc(100vh - 120px)"
        @change="handleContentChange"
      />
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import RichTextEditor from '@/components/RichTextEditor.vue'

const router = useRouter()
const route = useRoute()

const noteTitle = ref('')
const noteContent = ref('')
const saving = ref(false)

// 如果是编辑模式，加载笔记内容
const loadNote = async () => {
  if (route.params.id) {
    try {
      // 调用API获取笔记内容
      const response = await fetch(`/api/notes/${route.params.id}`)
      const data = await response.json()
      noteTitle.value = data.title
      noteContent.value = data.content
    } catch (error) {
      console.error('加载笔记失败:', error)
    }
  }
}

// 保存笔记
const handleSave = async () => {
  if (!noteTitle.value.trim()) {
    alert('请输入标题')
    return
  }

  saving.value = true
  try {
    const method = route.params.id ? 'PUT' : 'POST'
    const url = route.params.id 
      ? `/api/notes/${route.params.id}`
      : '/api/notes'

    const response = await fetch(url, {
      method,
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        title: noteTitle.value,
        content: noteContent.value
      })
    })

    if (response.ok) {
      router.push('/notes')
    }
  } catch (error) {
    console.error('保存失败:', error)
    alert('保存失败，请重试')
  } finally {
    saving.value = false
  }
}

const handleContentChange = (html) => {
  noteContent.value = html
}

// 加载笔记内容
loadNote()
</script>

<style scoped>
.note-edit-container {
  padding: 20px;
  height: 100vh;
  display: flex;
  flex-direction: column;
}

.note-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.note-title {
  font-size: 24px;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
  width: 70%;
}

.save-btn {
  padding: 8px 20px;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
}

.save-btn:hover:not(:disabled) {
  background-color: #45a049;
}

.save-btn:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.note-content {
  flex: 1;
  overflow: hidden;
}
</style> 