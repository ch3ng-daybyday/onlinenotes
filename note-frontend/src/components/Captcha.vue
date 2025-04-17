<template>
    <label for="captcha">验证码：</label>
    <img :src="captchaUrl" alt="验证码" @click="refreshCaptcha" />
    <input type="text" v-model="captcha" required />
</template>

<script>
import axios from 'axios';
export default {
    name:"CommentCaptcha",
    data() {
        return {
            captchaUrl: "",
            captcha: ""
        }
    },
    mounted() {
        this.refreshCaptcha();
    },
    methods: {
        async refreshCaptcha() {
            this.captchaUrl && URL.revokeObjectURL(this.captchaUrl);
            const response = await axios.get("https://localhost:7183/Account/GenerateGaptha", {
                responseType: 'blob',
                headers: { 'Access-Control-Allow-Origin': '*' }
            });
            this.captchaUrl = URL.createObjectURL(response.data);
        } 
    }
}
</script>

<style></style>