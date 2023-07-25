<template>
    <el-icon :size="size" :color="color"><component :is="name" /></el-icon>
</template>
<script lang="ts">
    import { defineComponent } from "vue";
    import * as Icons from '@element-plus/icons';

    export default defineComponent({
        components: Icons,
        name: "ElIcons",
        props: {
            name: {
                type: String,
                required: true,
            },
            size: {
                type: String,
            },
            color: {
                type: String,
            },
        }
    });
</script>