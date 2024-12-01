import { createRouter, createWebHistory } from 'vue-router';
import UserLogin from '@/views/account/UserLoginView.vue'; // 导入你的组件
import UserRegister from '@/views/account/UserRegisterView.vue'; 
 
const routes = [
    {
      path: '/login',
      name: 'UserLogin',
      component: UserLogin,
    },
    {
        path: '/register',
        name: 'UserRegister',
        component: UserRegister,
      }
  ];
  
  const router = createRouter({
    history: createWebHistory(), // 使用 HTML5 历史模式
    routes,
  });
  
  export default router;