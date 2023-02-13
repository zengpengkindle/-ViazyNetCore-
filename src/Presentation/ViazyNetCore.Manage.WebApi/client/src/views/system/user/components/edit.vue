<script lang="ts" setup>

</script>
<template>
<el-drawer
v-model="drawer"
title="I am the title"
:direction="direction"
:before-close="handleClose"
>
<span>Hi, there!</span>
</el-drawer>
</template>