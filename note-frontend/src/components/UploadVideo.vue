<template>
    <input type="file" @change="handlfileupload">
    <progress v-if="progress > 0" :value="progress" max="100"> </progress>
</template>
<script setup>
import { ref } from 'vue'
import axios from 'axios'
const progress = ref(0);
const handlfileupload = async (e) => {
    const file = e.target.files[0];
    if (!file) return;
    const formData = new FormData ();
    formData.append('file', file);
    try {
        const response = await axios.post('https://localhost:7137/api/Video/upload', formData, {
            headers: { 'Content-Type': 'multipart/form-data' },
            onUploadProgress: (progressEvent) => {
                progress.value = Math.round(
                    (progressEvent.loaded / progressEvent.total) * 100
                );
            }
        });
        console.log('Upload success:', response.data);
    } catch (error) {
        console.error('Upload failed:', error);

    }
}

</script>
