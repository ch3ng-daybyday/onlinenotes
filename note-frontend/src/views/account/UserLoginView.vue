<template>
  <div class="login">
    <form @submit.prevent="handleLogin">
      <div>
        <label for="username">用户名：</label>
        <input type="text" v-model="username" required />
      </div>
      <div>
        <label for="password">密码：</label>
        <input type="password" v-model="password" required />
      </div>
      <div v-if="CaptchEnabled">
        <label for="captcha">验证码：</label>
        <img :src="captchaUrl" alt="验证码" @click="refreshCaptcha" />
        <input type="text" v-model="captcha" required />
      </div>
      <button type="submit">登录</button>
      <p class="login-link">未有账号，请<router-link to="/register">注册</router-link></p>

    </form>
    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  data() {
    return {
      username: '',
      password: '',
      captcha: '',
      error: null,
    };
  },
  methods: {
    async handleLogin() {
      this.error = null;
      // 这里可以替换成你的 API 接口
      try {
        const response = await axios.post('https://localhost:7183/account/Login', {
          user: {
            userName: this.username,
            email: this.email,
          },
          PassWord: this.password,
          Captchakey: this.captcha,
        });
        if (response.data.status == "success") {
          if (response.data.meta.captcha.enabled) {
              this.CaptchEnabled = response.data.meta.captcha.enabled;
              this.refreshCaptcha();
            }
          if (response.data.meta.user.verified) {
            //login success
            localStorage.setItem("userInfo", response.data.token);
            this.error = response.data.token;
            this.$route.push({name: "home"});
          } else {
            console.log("login fail");
          }

        }
        // 处理成功后的逻辑，比如保存 token
        // console.log('登录成功', data);
      } catch (err) {
        this.error = err.message;
      }
    },
  },
};
</script>

<style scoped>
.login {
  max-width: 400px; /* 稍微增加宽度以适应更多的内容 */
  margin: 50px auto; /* 增加上下外边距以居中显示 */
  padding: 20px;
  background-color: #f9f9f9; /* 浅灰色背景 */
  border-radius: 8px; /* 圆角边框 */
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1); /* 轻微的阴影效果 */
}
 
.login form {
  display: flex;
  flex-direction: column;
}
 
.login label {
  margin-bottom: 8px; /* 标签和输入框之间的间距 */
  font-weight: bold; /* 加粗标签文本 */
  color: #333; /* 深灰色文本 */
}
 
.login input[type="text"],
.login input[type="password"],
.login input[type="text"] {
  width: 100%; /* 输入框宽度占满父容器 */
  padding: 10px; /* 内边距 */
  margin-bottom: 20px; /* 输入框之间的间距 */
  border: 1px solid #ccc; /* 灰色边框 */
  border-radius: 4px; /* 圆角边框 */
  box-sizing: border-box; /* 包括内边距和边框在内的宽度计算 */
}
 
.login input[type="text"]:focus,
.login input[type="password"]:focus,
.login input[type="text"]:focus {
  border-color: #80bdff; /* 聚焦时改变边框颜色 */
  outline: none; /* 移除默认的聚焦轮廓 */
  box-shadow: 0 0 0 .2rem rgba(0,123,255,.25); /* 添加聚焦阴影 */
}
 
.login button {
  width: 100%; /* 按钮宽度占满父容器 */
  padding: 10px; /* 内边距 */
  background-color: #007bff; /* 蓝色背景 */
  border: none; /* 移除边框 */
  border-radius: 4px; /* 圆角边框 */
  color: #fff; /* 白色文本 */
  font-size: 16px; /* 字体大小 */
  cursor: pointer; /* 鼠标悬停时显示手型 */
  transition: background-color .3s ease; /* 背景颜色过渡效果 */
}
 
.login button:hover {
  background-color: #0056b3; /* 鼠标悬停时改变背景颜色 */
}
 
.login .error {
  color: red; /* 红色文本 */
  margin-top: 10px; /* 错误消息和表单之间的间距 */
  font-size: 14px; /* 字体大小 */
}
 
/* 验证码图片样式 */
.login img {
  cursor: pointer; /* 鼠标悬停时显示手型，表示可点击 */
  margin-bottom: 10px; /* 图片和输入框之间的间距 */
}
</style>