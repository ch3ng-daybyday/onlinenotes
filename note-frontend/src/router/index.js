import { createRouter, createWebHistory } from 'vue-router';
import NoteListView from '@/views/notes/NoteListView.vue';
import NoteEditView from '@/views/notes/NoteEditView.vue';
import LoginView from '@/views/account/UserLoginView.vue';
import RegisterView from '@/views/account/UserRegisterView.vue';
import home from '@/views/home.vue';
const routes = [
  {
    path: '/login',
    name: 'login',
    component: LoginView
  }
  ,
  {
    path: '/register',
    name: 'register',
    component: RegisterView
  }
  ,
  {
    path: '/notes',
    name: 'notes',
    component: NoteListView
  },
  {
    path: '/notes/new',
    name: 'new-note',
    component: NoteEditView
  },
  {
    path: '/notes/:id/edit',
    name: 'edit-note',
    component: NoteEditView
  }
  ,
  {
    path: '/home',
    name: 'home',
    component: home
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
});

export default router;