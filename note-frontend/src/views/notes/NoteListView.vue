<template>
  <div class="notes-container">
    <!-- 移动端导航按钮 -->
    <div class="mobile-nav" v-if="isMobile">
      <button @click="toggleSidebar" class="nav-toggle">
        <span class="menu-icon">☰</span>
      </button>
      <h2 class="mobile-title">{{ currentCategory ? getCategoryName(currentCategory) : '所有笔记' }}</h2>
    </div>

    <!-- 左侧导航栏 -->
    <div class="sidebar" :class="{ 'show': showSidebar || !isMobile }">
      <div class="sidebar-header">
        <div class="search-box">
          <input 
            type="text" 
            v-model="searchQuery"
            placeholder="搜索笔记"
          />
        </div>
      </div>
      
      <div class="note-categories">
        <div 
          class="category-item" 
          :class="{ active: !currentCategory }" 
          @click="selectCategory(null)"
        >
          <span>所有笔记</span>
          <span class="count">{{ totalNotes }}</span>
        </div>
        <div 
          v-for="category in categories" 
          :key="category.id"
          class="category-item"
          :class="{ active: currentCategory === category.id }"
          @click="selectCategory(category.id)"
        >
          <span>{{ category.name }}</span>
          <span class="count">{{ category.count }}</span>
        </div>
      </div>
    </div>

    <!-- 中间笔记列表 -->
    <div class="notes-list" :class="{ 'show': showNotesList || !isMobile }">
      <div class="list-header">
        <button class="new-note-btn" @click="createNewNote">
          <span class="plus-icon">+</span> 新建笔记
        </button>
      </div>
      
      <div class="notes">
        <div 
          v-for="note in filteredNotes" 
          :key="note.id"
          class="note-item"
          :class="{ active: selectedNote === note.id }"
          @click="selectNote(note.id)"
        >
          <h3>{{ note.title }}</h3>
          <p class="note-excerpt">{{ note.excerpt }}</p>
          <div class="note-meta">
            <span>{{ formatDate(note.updatedAt) }}</span>
          </div>
        </div>
      </div>
    </div>

    <!-- 右侧编辑区域 -->
    <div class="editor-area" :class="{ 'show': showEditor || !isMobile }">
      <div class="editor-header" v-if="isMobile">
        <button class="back-btn" @click="closeEditor">
          <span>←</span>
        </button>
        <span class="current-note-title">{{ currentNoteTitle }}</span>
      </div>
      <RichTextEditor
        v-if="selectedNote"
        v-model="currentNoteContent"
        @change="handleContentChange"
      />
      <div v-else class="empty-state">
        选择或创建一个笔记开始编辑
      </div>
    </div>

    <!-- 移动端遮罩层 -->
    <div 
      v-if="isMobile && (showSidebar || (showNotesList && showEditor))" 
      class="mobile-overlay"
      @click="closeOverlay"
    ></div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted, onUnmounted } from 'vue'
import { useRouter } from 'vue-router'
import RichTextEditor from '@/components/RichTextEditor.vue'

const router = useRouter()
const searchQuery = ref('')
const currentCategory = ref(null)
const selectedNote = ref(null)
const currentNoteContent = ref('')
const isMobile = ref(false)
const showSidebar = ref(false)
const showNotesList = ref(true)
const showEditor = ref(false)

// 检查是否为移动设备
const checkMobile = () => {
  isMobile.value = window.innerWidth <= 768
}

onMounted(() => {
  checkMobile()
  window.addEventListener('resize', checkMobile)
})

onUnmounted(() => {
  window.removeEventListener('resize', checkMobile)
})

// 移动端交互方法
const toggleSidebar = () => {
  showSidebar.value = !showSidebar.value
  if (showSidebar.value) {
    showNotesList.value = false
    showEditor.value = false
  }
}

const selectCategory = (categoryId) => {
  currentCategory.value = categoryId
  if (isMobile.value) {
    showSidebar.value = false
    showNotesList.value = true
  }
}

const closeEditor = () => {
  showEditor.value = false
  showNotesList.value = true
}

const closeOverlay = () => {
  if (showSidebar.value) {
    showSidebar.value = false
  } else if (showNotesList.value && showEditor.value) {
    showNotesList.value = false
  }
}

// 模拟数据
const categories = ref([
  { id: 1, name: '工作', count: 5 },
  { id: 2, name: '学习', count: 3 },
  { id: 3, name: '生活', count: 2 },
])

const notes = ref([
  {
    id: 1,
    title: 'Vue 学习笔记',
    excerpt: '今天学习了Vue3的组合式API...',
    content: '',
    categoryId: 2,
    updatedAt: new Date()
  },
  // 更多笔记...
])

// 计算属性
const totalNotes = computed(() => notes.value.length)

const filteredNotes = computed(() => {
  return notes.value.filter(note => {
    const matchesCategory = !currentCategory.value || note.categoryId === currentCategory.value
    const matchesSearch = !searchQuery.value || 
      note.title.toLowerCase().includes(searchQuery.value.toLowerCase())
    return matchesCategory && matchesSearch
  })
})

// 方法
const formatDate = (date) => {
  return new Date(date).toLocaleDateString('zh-CN', {
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

const getCategoryName = (id) => {
  const category = categories.value.find(c => c.id === id)
  return category ? category.name : ''
}

const createNewNote = () => {
  router.push('/notes/new')
}

const selectNote = (noteId) => {
  selectedNote.value = noteId
  const note = notes.value.find(n => n.id === noteId)
  if (note) {
    currentNoteContent.value = note.content
    if (isMobile.value) {
      showEditor.value = true
    }
  }
}
const handleContentChange = (content) => {
  if (selectedNote.value) {
    const note = notes.value.find(n => n.id === selectedNote.value)
    if (note) {
      note.content = content
      // 这里可以添加自动保存逻辑
    }
  }
}
</script>

<style scoped>
.notes-container {
  display: flex;
  height: 100vh;
  background-color: #f5f5f5;
  position: relative;
  overflow: hidden;
}

/* 移动端导航样式 */
.mobile-nav {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  height: 50px;
  background-color: #fff;
  display: flex;
  align-items: center;
  padding: 0 16px;
  border-bottom: 1px solid #e0e0e0;
  z-index: 1000;
}

.nav-toggle {
  background: none;
  border: none;
  font-size: 24px;
  padding: 8px;
  cursor: pointer;
}

.mobile-title {
  margin-left: 16px;
  font-size: 18px;
}

/* 侧边栏样式 */
.sidebar {
  width: 250px;
  background-color: #fff;
  border-right: 1px solid #e0e0e0;
  display: flex;
  flex-direction: column;
  z-index: 200;
}

/* 笔记列表样式 */
.notes-list {
  width: 300px;
  background-color: #fff;
  border-right: 1px solid #e0e0e0;
  display: flex;
  flex-direction: column;
  z-index: 150;
}

/* 编辑区域样式 */
.editor-area {
  flex: 1;
  background-color: #fff;
  display: flex;
  flex-direction: column;
  z-index: 150;
}

/* 移动端响应式样式 */
@media (max-width: 768px) {
  .notes-container {
    flex-direction: column;
    padding-top: 50px;
  }

  .sidebar,
  .notes-list,
  .editor-area {
    position: fixed;
    top: 50px;
    left: 0;
    right: 0;
    bottom: 0;
    width: 100%;
    transform: translateX(-100%);
    transition: transform 0.3s ease;
  }

  .sidebar.show,
  .notes-list.show,
  .editor-area.show {
    transform: translateX(0);
    z-index: 200;
  }

  .mobile-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: rgba(0, 0, 0, 0.3);
    z-index: 100;
  }

  .editor-header {
    padding: 8px 16px;
    border-bottom: 1px solid #e0e0e0;
    display: flex;
    align-items: center;
  }

  .back-btn {
    background: none;
    border: none;
    font-size: 24px;
    padding: 8px;
    margin-right: 16px;
    cursor: pointer;
  }

  .current-note-title {
    font-size: 16px;
    font-weight: 500;
  }
}

.search-box {
  padding: 16px;
  border-bottom: 1px solid #e0e0e0;
}

.search-box input {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.note-categories {
  padding: 16px 0;
}

.category-item {
  padding: 8px 16px;
  display: flex;
  justify-content: space-between;
  cursor: pointer;
}

.category-item:hover {
  background-color: #f5f5f5;
}

.category-item.active {
  background-color: #e8f5e9;
  color: #4CAF50;
}

.notes-list {
  width: 300px;
  background-color: #fff;
  border-right: 1px solid #e0e0e0;
}

.list-header {
  padding: 16px;
  border-bottom: 1px solid #e0e0e0;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.new-note-btn {
  padding: 8px 16px;
  background-color: #4CAF50;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.notes {
  overflow-y: auto;
}

.note-item {
  padding: 16px;
  border-bottom: 1px solid #e0e0e0;
  cursor: pointer;
}

.note-item:hover {
  background-color: #f5f5f5;
}

.note-item.active {
  background-color: #e8f5e9;
}

.note-item h3 {
  margin: 0 0 8px 0;
  font-size: 16px;
}

.note-excerpt {
  color: #666;
  font-size: 14px;
  margin: 0 0 8px 0;
}

.note-meta {
  font-size: 12px;
  color: #999;
}

.editor-area {
  flex: 1;
  padding: 20px;
  background-color: #fff;
}

.empty-state {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  color: #999;
  font-size: 16px;
}
</style> 