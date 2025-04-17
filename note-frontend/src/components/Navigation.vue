<!-- src/views/Navigation.vue -->
<template>

    <div class="sidebar">
        <ul>
            <li><router-link to="/">首页</router-link></li>
            <li><router-link to="/home/Dashboard">仪表盘</router-link></li>
            <li><router-link to="/home/Setting">设置</router-link></li>
            <li><router-link to="/home/Profile">个人资料</router-link></li>
            <li><router-link to="/home/loginout">退出登录</router-link></li>
        </ul>
    </div>
    <div class="content">
        <!-- 这里是主要内容区域 -->
        <router-view></router-view>
    </div>
</template>

<script>
export default {
    name: 'NavigationComponent'
}
</script>

<style scoped>
body {
    font-family: Arial, sans-serif;
    margin: 0;
    padding: 0;
    display: flex;
}

.sidebar {
    width: 250px;
    height: 100vh;
    background-color: #333;
    color: white;
    padding-top: 20px;
    position: relative;
}

.sidebar ul {
    list-style-type: none;
    padding: 0;
}

.sidebar li {
    padding: 15px;
    text-align: center;
}

.sidebar a {
    color: white;
    text-decoration: none;
    display: block;
    width: 100%;
}

.sidebar a:hover {
    background-color: #575757;
}

.content {
    flex-grow: 1;
    padding: 20px;
    background-color: #f4f4f4;
}
</style>