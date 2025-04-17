<template>
  <div>
    <canvas class="signature-canvas" ref="canvas" @touchstart="startDrawing" @touchmove="draw" @touchend="endDrawing"
      @mousedown="startDrawing" @mousemove="draw" @mouseup="endDrawing"></canvas>
    <button @click="saveSignature">保存签名</button>
    <button @click="clearCanvas">清空画布</button> <!-- 新增按钮 -->
  </div>
</template>
<style scoped>
.signature-canvas {
  background-color: #f0f0f0;
}
</style>
<script>
export default {
  name: 'DocumentSignature',
  data() {
    return {
      isDrawing: false,
      ctx: null,
      lastX: 0,
      lastY: 0,

    };
  },
  mounted() {
    const canvas = this.$refs.canvas;
    canvas.width = 300;
    canvas.height = 150;
    this.ctx = canvas.getContext('2d');
    this.ctx.strokeStyle = '#000';
    this.ctx.lineWidth = 2;
  },
  methods: {
    startDrawing(e) {
      this.isDrawing = true;
      const rect = e.target.getBoundingClientRect();
      this.lastX = (e.touches?.[0].clientX || e.clientX) - rect.left;
      this.lastY = (e.touches?.[0].clientY || e.clientY) - rect.top;
    },
    draw(e) {
      if (!this.isDrawing) return;
      const rect = e.target.getBoundingClientRect();
      const x = (e.touches?.[0].clientX || e.clientX) - rect.left;
      const y = (e.touches?.[0].clientY || e.clientY) - rect.top;

      this.ctx.beginPath();
      this.ctx.moveTo(this.lastX, this.lastY);
      this.ctx.lineTo(x, y);
      this.ctx.stroke();

      this.lastX = x;
      this.lastY = y;
    },
    endDrawing() {
      this.isDrawing = false;
    },
    saveSignature() {
      const canvas = this.$refs.canvas;
      const image = canvas.toDataURL('image/png'); // 生成Base64图片
      this.$emit('save', image); // 传递给后端
    },
    clearCanvas() {
      const canvas = this.$refs.canvas;
      this.ctx.clearRect(0, 0, canvas.width, canvas.height); // 清空画布
      
      // 如果之前设置了背景色，需要重新填充
      this.ctx.fillStyle = "#f0f0f0"; // 与背景色一致
      this.ctx.fillRect(0, 0, canvas.width, canvas.height);
      
      // 重置画笔属性（避免被覆盖）
      this.ctx.strokeStyle = '#000';
      this.ctx.lineWidth = 2;
    }
  }
};
</script>