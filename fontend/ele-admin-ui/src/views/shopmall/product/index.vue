<template title="商品管理">
  <div class="main">
    <div>
      <x-table :params="params" ref="table" :url="findAll">
        <template slot="form">
          <el-form-item label="标题">
            <el-input
              placeholder="模糊搜索"
              v-model="params.titleLike"
            ></el-input>
          </el-form-item>

          <el-form-item label="类目">
            <el-input
              placeholder="精准搜索"
              v-model="params.catName"
            ></el-input>
          </el-form-item>
          <el-form-item label="前台隐藏">
            <el-select v-model="params.isHidden" placeholder="全部">
              <el-option
                v-for="r in options"
                :key="r.value"
                :label="r.label"
                :value="r.value"
              >
              </el-option>
            </el-select>
          </el-form-item>
          <el-form-item label="状态">
            <el-select v-model="params.status" placeholder="全部">
              <el-option
                v-for="r in statusOptions"
                :key="r.value"
                :label="r.label"
                :value="r.value"
              >
              </el-option>
            </el-select>
          </el-form-item>
          <el-form-item label="创建时间">
            <el-date-picker
              v-model="params.createTimes"
              type="daterange"
              :picker-options="$pickerOptions"
              range-separator="至"
              start-placeholder="开始日期"
              end-placeholder="结束日期"
              align="right"
            >
            </el-date-picker>
          </el-form-item>
          <el-row>
            <el-col :span="18">
              <el-dropdown @command="dropCmdClick">
                <el-button type="primary">
                  新增商品<i class="el-icon-arrow-down el-icon--right"></i>
                </el-button>
                <el-dropdown-menu slot="dropdown">
                  <el-dropdown-item
                    v-for="opt in productOuters"
                    :command="opt.value"
                    >{{ opt.label }}</el-dropdown-item
                  >
                </el-dropdown-menu>
              </el-dropdown>
              <el-button
                type="success"
                round
                icon="el-icon-plus"
                @click="$router.push('/product/manage')"
                >添加商品</el-button
              >
            </el-col>
            <el-col :span="6" align="right">
              <el-button
                type="primary"
                icon="el-icon-search"
                native-type="submit"
                >搜索</el-button
              >
            </el-col>
          </el-row>
        </template>
        <el-table-column label="标题" fixed width="380">
          <template slot-scope="scope">
            <el-row :gutter="20">
              <el-col :span="8">
                <img :src="scope.row.image" width="120" />
              </el-col>
              <el-col :span="16">
                <p class="product_title">
                  {{ scope.row.title }}
                </p>
                <p>
                  <span>{{ scope.row.catPath }}</span>
                </p>
              </el-col>
            </el-row>
          </template>
        </el-table-column>

        <el-table-column
          prop="catName"
          label="类目"
          width="80"
        ></el-table-column>
        <el-table-column
          prop="shopName"
          label="商家"
          width="80"
        ></el-table-column>
        <el-table-column prop="cost" label="成本" width="80"></el-table-column>
        <el-table-column prop="price" label="售价" width="80"></el-table-column>
        <!--<el-table-column prop="stockNum" label="总库存" width="80"></el-table-column>
            <el-table-column prop="totalSellNum" label="销量" width="80"></el-table-column>-->
        <el-table-column label="前台隐藏">
          <template slot-scope="scope">
            <span v-if="scope.row.isHidden">是</span>
            <span v-else>否</span>
          </template>
        </el-table-column>
        <el-table-column label="状态">
          <template slot-scope="scope">
            <span v-if="scope.row.status == 0">未上架</span>
            <span v-else-if="scope.row.status == 1">待审核</span>
            <span v-else-if="scope.row.status == 2">出售中</span>
          </template>
        </el-table-column>
        <el-table-column
          prop="createTime"
          width="200"
          label="创建时间"
          :formatter="$dateFormatter"
        ></el-table-column>
        <el-table-column label="操作" width="100" fixed="right">
          <template slot-scope="scope">
            <router-link
              :to="{
                path: '/product/manage',
                query: { id: scope.row.id, outerType: scope.row.outerType }
              }"
              target="_blank"
              >编辑</router-link
            >
            <router-link
              :to="{ path: '/stock/manage', query: { id: scope.row.id } }"
              target="_blank"
              >库存</router-link
            >
            <el-button
              @click="modifyStatus(scope.row, 2)"
              v-if="scope.row.status == 0"
              type="text"
              size="small"
              >上架</el-button
            >
            <el-button
              @click="modifyStatus(scope.row, 0)"
              v-if="scope.row.status == 2"
              type="text"
              size="small"
              >下架</el-button
            >
            <el-button
              @click="remove(scope.row.id)"
              v-if="scope.row.status == 0"
              type="text"
              size="small"
              >删除</el-button
            >
          </template>
        </el-table-column>
      </x-table>
    </div>
  </div>
</template>
<script>
import { product, productOuter } from "../../apis";

export default {
  data() {
    return {
      findAll: product.findAll,

      params: {},
      statusOptions: [
        {
          value: "",
          label: "全部"
        },
        {
          value: 0,
          label: "未上架"
        },
        {
          value: 1,
          label: "待审核"
        },
        {
          value: 2,
          label: "出售中"
        }
      ],
      options: [
        {
          value: false,
          label: "否"
        },
        {
          value: true,
          label: "是"
        }
      ],
      productOuters: []
    };
  },
  keepAlive: true,
  created() {
    var vm = this;
    productOuter.getAllAsync().then(cr => {
      var list = cr.value;
      vm.productOuters = [{ value: "", label: "普通商品" }];
      var outers = Object.keys(list).map(function (listCode) {
        return {
          value: listCode,
          label: list[listCode]
        };
      });
      vm.productOuters = [...vm.productOuters, ...outers];
    });
  },
  methods: {
    remove(id) {
      var vm = this;
      msg.confirm("确定删除？", function () {
        product.remove(id).then(r => {
          if (r.status) {
            vm.$alert(r.message, { type: "error" });
          } else {
            vm.$message("删除成功！");
            vm.$refs.table.refresh();
          }
        });
      });
    },
    dropCmdClick(cmd) {
      this.$router.push("./manage?outerType=" + cmd);
    },
    modifyStatus(row, status) {
      var vm = this;
      product.modifyStatus(row.id, status).then(r => {
        if (r.status) {
          vm.$alert(r.message, { type: "error" });
        } else {
          vm.$message("操作成功！");
          vm.$refs.table.refresh();
        }
      });
    }
  }
};
</script>
