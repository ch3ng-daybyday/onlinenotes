<template>
    <div class="upload-container">
      <!-- 文件选择 -->
      <input 
        type="file" 
        ref="fileInput" 
        @change="handleFileSelect" 
        accept="video/*"
      >
      <button @click="triggerFileSelect">选择视频文件</button>
  
      <!-- 上传控制 -->
      <button 
        @click="startUpload" 
        :disabled="isUploading"
      >
        {{ isUploading ? '上传中...' : '开始上传' }}
      </button>
  
      <!-- 进度显示 -->
      <div v-if="progress > 0" class="progress-container">
        <progress :value="progress" max="100"></progress>
        <span>{{ progress.toFixed(1) }}%</span>
      </div>
  
      <!-- 错误提示 -->
      <div v-if="errorMessage" class="error-message">
        {{ errorMessage }}
      </div>
  
      <!-- 上传完成提示 -->
      <div v-if="uploadedUrl" class="success-message">
        上传成功！视频地址：<a :href="uploadedUrl" target="_blank">{{ uploadedUrl }}</a>
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from 'vue';
  import axios from 'axios';
  
  const CHUNK_SIZE = 10 * 1024 * 1024; // 5MB 分块
  const fileInput = ref(null);
  const isUploading = ref(false);
  const progress = ref(0);
  const errorMessage = ref('');
  const uploadedUrl = ref('');
  const sessionId = ref('');    
// 生成唯一 sessionId（更可靠的方法）
const generateSessionId = () => {
  // 使用时间戳 + 随机数组合
  return Date.now().toString(36) + Math.random().toString(36).substring(2);
};

  // 触发文件选择
  const triggerFileSelect = () => {
    fileInput.value.click();
  };
  
  // 选择文件回调
  const handleFileSelect = (e) => {
    const file = e.target.files[0];
    if (!file) return;
    resetState();
  };
  
  // 重置状态
  const resetState = () => {
    progress.value = 0;
    errorMessage.value = '';
    uploadedUrl.value = '';
  };
  
  // 开始上传
  const startUpload = async () => {
    const file = fileInput.value.files[0];
    if (!file) {
      errorMessage.value = '请先选择文件';
      return;
    }
  
    isUploading.value = true;
    errorMessage.value = '';
    sessionId.value =  generateSessionId(); 
    try {
      // 分块上传
      const totalChunks = Math.ceil(file.size / CHUNK_SIZE);
      let uploadedChunks = 0;
  
      for (let chunkIdx = 0; chunkIdx < totalChunks; chunkIdx++) {
        const start = chunkIdx * CHUNK_SIZE;
        const end = Math.min(start + CHUNK_SIZE, file.size);
        const chunk = file.slice(start, end);
  
        const formData = new FormData();
        formData.append('file', chunk);
        formData.append('sessionId',sessionId.value);
        formData.append('chunkIndex', chunkIdx);
        formData.append('totalChunks', totalChunks);
        formData.append('fileName', file.name);
        await axios.post('https://localhost:7137/api/Video/upload-chunk', formData, {
          headers: { 'Content-Type': 'multipart/form-data' },
          onUploadProgress: (progressEvent) => {
            // 计算整体进度
            const chunkProgress = (uploadedChunks + progressEvent.loaded / progressEvent.total) / totalChunks * 100;
            progress.value = Math.min(chunkProgress, 100);
          }
        });
  
        uploadedChunks++;
      }
  
      // 获取最终合并后的 URL
      const response = await axios.post('https://localhost:7137/api/Video/merge',{
        sessionId: sessionId.value,
        fileName: file.name,
        totalChunk:totalChunks
      });
      console.log('合并完成',response.data.path);
    //   uploadedUrl.value = response.data.url;
  
    } catch (error) {
      errorMessage.value = `上传失败: ${error.response?.data || error.message}`;
    } finally {
      isUploading.value = false;
    }
  };
  </script>
  
  <style scoped>
  .upload-container {
    max-width: 500px;
    margin: 20px auto;
    padding: 20px;
    border: 1px solid #eee;
  }
  
  .progress-container {
    margin-top: 15px;
  }
  
  progress {
    width: 300px;
    height: 20px;
  }
  
  .error-message {
    color: #ff4444;
    margin-top: 10px;
  }
  
  .success-message {
    color: #00C851;
    margin-top: 10px;
  }
  </style>