<template>
    <video controls @error="handleVideoError">
        <source :src="videoUrl" type="video/mp4">
    </video>
</template>
<script setup>
import { ref } from "vue";
const videoUrl = ref('https://localhost:7137/api/Video/PlayVideo/sample.mp4');
const handleVideoError = () => {
    // 避免无限重试：错误时替换为备用URL或停止加载
    videoUrl.value = ''; // 清空地址或设置备用视频地址
};
</script>