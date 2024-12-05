<template>
  <div class="register-container">
    <h2>注册账号</h2>
    <form @submit.prevent="handleRegister">
      <div class="form-group">
        <label for="username">用户名</label>
        <input type="text" id="username" v-model="username" required />
      </div>

      <div class="form-group">
        <label for="email">邮箱</label>
        <input type="email" id="email" v-model="email" required />
      </div>

      <div class="form-group">
        <label for="password">密码</label>
        <input type="password" id="password" v-model="password" required />
      </div>
      <div v-if="CaptchEnabled">
        <label for="captcha">验证码：</label>
        <img :src="captchaUrl" alt="验证码" @click="refreshCaptcha" />
        <input type="text" v-model="captcha" required />
      </div>
      <button type="submit">注册</button>
      <p class="login-link">已有账号？<router-link to="/login">登录</router-link></p>
    </form>
  </div>
</template>

<script>
import axios from 'axios';
export default {
  data() {
    return {
      username: '',
      email: '',
      password: '',
      captcha: '', // 定义 captcha 用于绑定验证码输入框
      captchaUrl: '',// 定义 captchaUrl 用于存储验证码图片的 URL
      CaptchEnabled: false
    };
  },
  mounted() {
    this.refreshCaptcha();
  },
  methods: {
    async handleRegister() {
      try {
        const response = await axios.post("https://localhost:7183/Account/Register", {
          user: {
            userName: this.username,
            email: this.email,
            emailConfirmed: true
          },
          PassWord: this.password,
          Captchakey: this.captcha,
        });
        console.log(response);
        if (response.data.status == "success") {
          //跳转页面
          if (response.data.meta.captcha.enabled) {
            this.CaptchEnabled = response.data.meta.captcha.enabled;
            this.refreshCaptcha();
            return;
          }
          if (response.data.meta.user.verified) {
            //login success
            localStorage.setItem("userInfo", response.data.token);
            this.error = response.data.token;
            this.$router.push({ name: "home" });
          }
        } else if (response.data.status == "fail") {
          console.log(response.data.message);
        }
        else {
          console.error("发生预料之外的错误");
        }
      }
      catch (error) {
        console.log(error);
        // throw new Error('注册失败');
      }
    },
    async refreshCaptcha() {
      this.captchaUrl && URL.revokeObjectURL(this.captchaUrl);
      const response = await axios.get("https://localhost:7183/Account/GenerateGaptha", {
        responseType: 'blob',
        headers: { 'Access-Control-Allow-Origin': '*' }
      });
      this.captchaUrl = URL.createObjectURL(response.data);
    }
  }
};
</script>

<style scoped>
.register-container {
  max-width: 400px;
  margin: 0 auto;
  padding: 20px;
  background-color: #fff;
  border-radius: 8px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  text-align: center;
}

h2 {
  margin-bottom: 20px;
  color: #333;
}

.form-group {
  margin-bottom: 15px;
  text-align: left;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.form-group input {
  width: 100%;
  padding: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
  box-sizing: border-box;
}

button {
  width: 100%;
  padding: 10px;
  background-color: #0095f6;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

button:hover {
  background-color: #007bbf;
  /* 悬停效果 */
}

.login-link {
  margin-top: 10px;
  color: #007bbf;
}

.login-link a {
  text-decoration: none;
  color: #0095f6;
}
</style>