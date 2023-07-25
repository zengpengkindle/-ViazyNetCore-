<template>
  <div id="changeContab">
    <el-tabs type="border-card">
      <el-tab-pane>
        <template #label>
          <span><i class="el-icon-date" /> {{ text.Seconds.name }}</span>
        </template>
        <div class="tabBody">
          <el-row>
            <el-radio v-model="second.cronEvery" label="1">{{
              text.Seconds.every
            }}</el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="second.cronEvery" label="2"
              >{{ text.Seconds.interval[0] }}
              <el-input-number
                size="small"
                v-model="second.incrementIncrement"
                :min="1"
                :max="60"
              />
              {{ text.Seconds.interval[1] || "" }}
              <el-input-number
                size="small"
                v-model="second.incrementStart"
                :min="0"
                :max="59"
              />
              {{ text.Seconds.interval[2] || "" }}
            </el-radio>
          </el-row>
          <el-row>
            <el-radio class="long" v-model="second.cronEvery" label="3"
              >{{ text.Seconds.specific }}
              <el-select
                size="small"
                multiple
                v-model="second.specificSpecific"
              >
                <el-option
                  v-for="(val, index) in 60"
                  :key="index"
                  :value="val - 1"
                  >{{ val - 1 }}</el-option
                >
              </el-select>
            </el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="second.cronEvery" label="4"
              >{{ text.Seconds.cycle[0] }}
              <el-input-number
                size="small"
                v-model="second.rangeStart"
                :min="1"
                :max="60"
              />
              {{ text.Seconds.cycle[1] || "" }}
              <el-input-number
                size="small"
                v-model="second.rangeEnd"
                :min="0"
                :max="59"
              />
              {{ text.Seconds.cycle[2] || "" }}
            </el-radio>
          </el-row>
        </div>
      </el-tab-pane>
      <el-tab-pane>
        <template #label>
          <span><i class="el-icon-date" /> {{ text.Minutes.name }}</span>
        </template>
        <div class="tabBody">
          <el-row>
            <el-radio v-model="minute.cronEvery" label="1">{{
              text.Minutes.every
            }}</el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="minute.cronEvery" label="2"
              >{{ text.Minutes.interval[0] }}
              <el-input-number
                size="small"
                v-model="minute.incrementIncrement"
                :min="1"
                :max="60"
              />
              {{ text.Minutes.interval[1] }}
              <el-input-number
                size="small"
                v-model="minute.incrementStart"
                :min="0"
                :max="59"
              />
              {{ text.Minutes.interval[2] || "" }}
            </el-radio>
          </el-row>
          <el-row>
            <el-radio class="long" v-model="minute.cronEvery" label="3"
              >{{ text.Minutes.specific }}
              <el-select
                size="small"
                multiple
                v-model="minute.specificSpecific"
              >
                <el-option
                  v-for="(val, index) in 60"
                  :key="index"
                  :value="val - 1"
                  >{{ val - 1 }}</el-option
                >
              </el-select>
            </el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="minute.cronEvery" label="4"
              >{{ text.Minutes.cycle[0] }}
              <el-input-number
                size="small"
                v-model="minute.rangeStart"
                :min="1"
                :max="60"
              />
              {{ text.Minutes.cycle[1] }}
              <el-input-number
                size="small"
                v-model="minute.rangeEnd"
                :min="0"
                :max="59"
              />
              {{ text.Minutes.cycle[2] }}
            </el-radio>
          </el-row>
        </div>
      </el-tab-pane>
      <el-tab-pane>
        <template #label>
          <span><i class="el-icon-date" /> {{ text.Hours.name }}</span>
        </template>
        <div class="tabBody">
          <el-row>
            <el-radio v-model="hour.cronEvery" label="1">{{
              text.Hours.every
            }}</el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="hour.cronEvery" label="2"
              >{{ text.Hours.interval[0] }}
              <el-input-number
                size="small"
                v-model="hour.incrementIncrement"
                :min="0"
                :max="23"
              />
              {{ text.Hours.interval[1] }}
              <el-input-number
                size="small"
                v-model="hour.incrementStart"
                :min="0"
                :max="23"
              />
              {{ text.Hours.interval[2] }}
            </el-radio>
          </el-row>
          <el-row>
            <el-radio class="long" v-model="hour.cronEvery" label="3"
              >{{ text.Hours.specific }}
              <el-select size="small" multiple v-model="hour.specificSpecific">
                <el-option
                  v-for="(val, index) in 24"
                  :key="index"
                  :value="val - 1"
                  >{{ val - 1 }}</el-option
                >
              </el-select>
            </el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="hour.cronEvery" label="4"
              >{{ text.Hours.cycle[0] }}
              <el-input-number
                size="small"
                v-model="hour.rangeStart"
                :min="0"
                :max="23"
              />
              {{ text.Hours.cycle[1] }}
              <el-input-number
                size="small"
                v-model="hour.rangeEnd"
                :min="0"
                :max="23"
              />
              {{ text.Hours.cycle[2] }}
            </el-radio>
          </el-row>
        </div>
      </el-tab-pane>
      <el-tab-pane>
        <template #label>
          <span><i class="el-icon-date" /> {{ text.Day.name }}</span>
        </template>
        <div class="tabBody">
          <el-row>
            <el-radio v-model="day.cronEvery" label="1">{{
              text.Day.every
            }}</el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="day.cronEvery" label="2"
              >{{ text.Day.intervalWeek[0] }}
              <el-input-number
                size="small"
                v-model="week.incrementIncrement"
                :min="1"
                :max="7"
              />
              {{ text.Day.intervalWeek[1] }}
              <el-select size="small" v-model="week.incrementStart">
                <el-option
                  v-for="(val, index) in 7"
                  :key="index"
                  :label="text.Week[val - 1]"
                  :value="val"
                />
              </el-select>
              {{ text.Day.intervalWeek[2] }}
            </el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="day.cronEvery" label="3"
              >{{ text.Day.intervalDay[0] }}
              <el-input-number
                size="small"
                v-model="day.incrementIncrement"
                :min="1"
                :max="31"
              />
              {{ text.Day.intervalDay[1] }}
              <el-input-number
                size="small"
                v-model="day.incrementStart"
                :min="1"
                :max="31"
              />
              {{ text.Day.intervalDay[2] }}
            </el-radio>
          </el-row>
          <el-row>
            <el-radio class="long" v-model="day.cronEvery" label="4"
              >{{ text.Day.specificWeek }}
              <el-select size="small" multiple v-model="week.specificSpecific">
                <el-option
                  v-for="(val, index) in 7"
                  :key="index"
                  :label="text.Week[val - 1]"
                  :value="
                    ['SUN', 'MON', 'TUE', 'WED', 'THU', 'FRI', 'SAT'][val - 1]
                  "
                />
              </el-select>
            </el-radio>
          </el-row>
          <el-row>
            <el-radio class="long" v-model="day.cronEvery" label="5"
              >{{ text.Day.specificDay }}
              <el-select size="small" multiple v-model="day.specificSpecific">
                <el-option
                  v-for="(val, index) in 31"
                  :key="index"
                  :value="val"
                  >{{ val }}</el-option
                >
              </el-select>
            </el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="day.cronEvery" label="6">{{
              text.Day.lastDay
            }}</el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="day.cronEvery" label="7">{{
              text.Day.lastWeekday
            }}</el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="day.cronEvery" label="8"
              >{{ text.Day.lastWeek[0] }}
              <el-select size="small" v-model="day.cronLastSpecificDomDay">
                <el-option
                  v-for="(val, index) in 7"
                  :key="index"
                  :label="text.Week[val - 1]"
                  :value="val"
                />
              </el-select>
              {{ text.Day.lastWeek[1] || "" }}
            </el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="day.cronEvery" label="9">
              <el-input-number
                size="small"
                v-model="day.cronDaysBeforeEomMinus"
                :min="1"
                :max="31"
              />
              {{ text.Day.beforeEndMonth[0] }}
            </el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="day.cronEvery" label="10"
              >{{ text.Day.nearestWeekday[0] }}
              <el-input-number
                size="small"
                v-model="day.cronDaysNearestWeekday"
                :min="1"
                :max="31"
              />
              {{ text.Day.nearestWeekday[1] }}
            </el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="day.cronEvery" label="11"
              >{{ text.Day.someWeekday[0] }}
              <el-input-number
                size="small"
                v-model="week.cronNthDayNth"
                :min="1"
                :max="5"
              />
              <el-select size="small" v-model="week.cronNthDayDay">
                <el-option
                  v-for="(val, index) in 7"
                  :key="index"
                  :label="text.Week[val - 1]"
                  :value="val"
                />
              </el-select>
              {{ text.Day.someWeekday[1] }}
            </el-radio>
          </el-row>
        </div>
      </el-tab-pane>
      <el-tab-pane>
        <template #label>
          <span><i class="el-icon-date" /> {{ text.Month.name }}</span>
        </template>
        <div class="tabBody">
          <el-row>
            <el-radio v-model="month.cronEvery" label="1">{{
              text.Month.every
            }}</el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="month.cronEvery" label="2"
              >{{ text.Month.interval[0] }}
              <el-input-number
                size="small"
                v-model="month.incrementIncrement"
                :min="0"
                :max="12"
              />
              {{ text.Month.interval[1] }}
              <el-input-number
                size="small"
                v-model="month.incrementStart"
                :min="0"
                :max="12"
              />
            </el-radio>
          </el-row>
          <el-row>
            <el-radio class="long" v-model="month.cronEvery" label="3"
              >{{ text.Month.specific }}
              <el-select size="small" multiple v-model="month.specificSpecific">
                <el-option
                  v-for="(val, index) in 12"
                  :key="index"
                  :label="val"
                  :value="val"
                />
              </el-select>
            </el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="month.cronEvery" label="4"
              >{{ text.Month.cycle[0] }}
              <el-input-number
                size="small"
                v-model="month.rangeStart"
                :min="1"
                :max="12"
              />
              {{ text.Month.cycle[1] }}
              <el-input-number
                size="small"
                v-model="month.rangeEnd"
                :min="1"
                :max="12"
              />
            </el-radio>
          </el-row>
        </div>
      </el-tab-pane>
      <el-tab-pane>
        <template #label>
          <span><i class="el-icon-date" /> {{ text.Year.name }}</span>
        </template>
        <div class="tabBody">
          <el-row>
            <el-radio v-model="year.cronEvery" label="1">{{
              text.Year.every
            }}</el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="year.cronEvery" label="2"
              >{{ text.Year.interval[0] }}
              <el-input-number
                size="small"
                v-model="year.incrementIncrement"
                :min="1"
                :max="99"
              />
              {{ text.Year.interval[1] }}
              <el-input-number
                size="small"
                v-model="year.incrementStart"
                :min="nowYear"
                :max="nowYear + 100"
              />
            </el-radio>
          </el-row>
          <el-row>
            <el-radio class="long" v-model="year.cronEvery" label="3"
              >{{ text.Year.specific }}
              <el-select
                size="small"
                filterable
                multiple
                v-model="year.specificSpecific"
              >
                <el-option
                  v-for="(val, index) in 100"
                  :key="index"
                  :label="nowYear + val - 1"
                  :value="nowYear + val - 1"
                />
              </el-select>
            </el-radio>
          </el-row>
          <el-row>
            <el-radio v-model="year.cronEvery" label="4"
              >{{ text.Year.cycle[0] }}
              <el-input-number
                size="small"
                v-model="year.rangeStart"
                :min="nowYear"
                :max="nowYear + 100"
              />
              {{ text.Year.cycle[1] }}
              <el-input-number
                size="small"
                v-model="year.rangeEnd"
                :min="nowYear"
                :max="nowYear + 100"
              />
            </el-radio>
          </el-row>
        </div>
      </el-tab-pane>
    </el-tabs>
    <el-card class="box-card">
      <el-row :gutter="5">
        <el-col :span="3">
          <div style="margin-top: 15px">
            <el-input placeholder="请输入内容" v-model="secondsText">
              <template #append>{{ text.Seconds.name }}</template>
            </el-input>
          </div>
        </el-col>
        <el-col :span="3">
          <div style="margin-top: 15px">
            <el-input placeholder="请输入内容" v-model="minutesText">
              <template #append>{{ text.Minutes.name }}</template>
            </el-input>
          </div>
        </el-col>
        <el-col :span="3">
          <div style="margin-top: 15px">
            <el-input placeholder="请输入内容" v-model="hoursText">
              <template #append>{{ text.Hours.name }}</template>
            </el-input>
          </div>
        </el-col>
        <el-col :span="3">
          <div style="margin-top: 15px">
            <el-input placeholder="请输入内容" v-model="daysText">
              <template #append>{{ text.Day.name }}</template>
            </el-input>
          </div>
        </el-col>
        <el-col :span="3">
          <div style="margin-top: 15px">
            <el-input placeholder="请输入内容" v-model="weeksText">
              <template #append>周</template>
            </el-input>
          </div>
        </el-col>
        <el-col :span="3">
          <div style="margin-top: 15px">
            <el-input placeholder="请输入内容" v-model="monthsText">
              <template #append>{{ text.Month.name }}</template>
            </el-input>
          </div>
        </el-col>
        <el-col :span="3">
          <div style="margin-top: 15px">
            <el-input placeholder="请输入内容" v-model="yearsText">
              <template #append>{{ text.Year.name }}</template>
            </el-input>
          </div>
        </el-col>
      </el-row>
      <div class="value">{{ this.cron }}</div>
    </el-card>
    <div class="bottom">
      <el-button type="primary" @click="change">{{ text.Save }}</el-button>
      <el-button type="primary" @click="close">{{ text.Close }}</el-button>
    </div>
  </div>
</template>
<script>
import language from "./language";
export default {
  name: "vueCron",
  props: ["data", "formData"],
  data() {
    return {
      nowYear: 0,
      second: {
        cronEvery: "",
        incrementStart: "3",
        incrementIncrement: "5",
        rangeStart: "",
        rangeEnd: "",
        specificSpecific: []
      },
      minute: {
        cronEvery: "",
        incrementStart: "3",
        incrementIncrement: "5",
        rangeStart: "",
        rangeEnd: "",
        specificSpecific: []
      },
      hour: {
        cronEvery: "",
        incrementStart: "3",
        incrementIncrement: "5",
        rangeStart: "",
        rangeEnd: "",
        specificSpecific: []
      },
      day: {
        cronEvery: "",
        incrementStart: "1",
        incrementIncrement: "1",
        rangeStart: "",
        rangeEnd: "",
        specificSpecific: [],
        cronLastSpecificDomDay: 1,
        cronDaysBeforeEomMinus: "",
        cronDaysNearestWeekday: ""
      },
      week: {
        cronEvery: "",
        incrementStart: "1",
        incrementIncrement: "1",
        specificSpecific: [],
        cronNthDayDay: 1,
        cronNthDayNth: "1"
      },
      month: {
        cronEvery: "",
        incrementStart: "3",
        incrementIncrement: "5",
        rangeStart: "",
        rangeEnd: "",
        specificSpecific: []
      },
      year: {
        cronEvery: "",
        incrementStart: "",
        incrementIncrement: "1",
        rangeStart: "",
        rangeEnd: "",
        specificSpecific: []
      },
      output: {
        second: "",
        minute: "",
        hour: "",
        day: "",
        month: "",
        Week: "",
        year: ""
      }
    };
  },
  watch: {
    data() {
      this.rest(this.$data);
    }
  },
  computed: {
    text() {
      return language;
    },
    secondsText: {
      get() {
        let seconds = "";
        const cronEvery = this.second.cronEvery;
        switch (cronEvery.toString()) {
          case "1":
            seconds = "*";
            break;
          case "2":
            seconds =
              this.second.incrementStart + "/" + this.second.incrementIncrement;
            break;
          case "3":
            this.second.specificSpecific.map(val => {
              seconds += val + ",";
            });
            seconds = seconds.slice(0, -1);
            break;
          case "4":
            seconds = this.second.rangeStart + "-" + this.second.rangeEnd;
            break;
        }
        return seconds;
      },
      set(value) {
        if (!value) return;
        if (value === "*") {
          this.second.cronEvery = "1";
          return;
        }
        if (value.indexOf("/") !== -1) {
          this.second.cronEvery = "2";
          const valueArr = value.split("/");
          this.second.incrementStart = Number(valueArr[0]);
          this.second.incrementIncrement = Number(valueArr[1]);
          return;
        }
        if (
          value.indexOf(",") !== -1 ||
          (value.length > 0 &&
            !Number.isNaN(Number(value)) &&
            Number(value) < 60 &&
            Number(value) >= 0)
        ) {
          this.second.cronEvery = "3";
          const valueArr = value.split(",");
          this.second.specificSpecific = valueArr.map(item => Number(item));
          return;
        }
        if (value.indexOf("-") !== -1) {
          this.second.cronEvery = "4";
          const valueArr = value.split("-");
          this.second.rangeStart = Number(valueArr[0]);
          this.second.rangeEnd = Number(valueArr[1]);
          return;
        }
      }
    },
    minutesText: {
      get() {
        let minutes = "";
        const cronEvery = this.minute.cronEvery;
        switch (cronEvery.toString()) {
          case "1":
            minutes = "*";
            break;
          case "2":
            minutes =
              this.minute.incrementStart + "/" + this.minute.incrementIncrement;
            break;
          case "3":
            this.minute.specificSpecific.map(val => {
              minutes += val + ",";
            });
            minutes = minutes.slice(0, -1);
            break;
          case "4":
            minutes = this.minute.rangeStart + "-" + this.minute.rangeEnd;
            break;
        }
        return minutes;
      },
      set(value) {
        if (!value) return;
        if (value === "*") {
          this.minute.cronEvery = "1";
          return;
        }
        if (value.indexOf("/") !== -1) {
          this.minute.cronEvery = "2";
          const valueArr = value.split("/");
          this.minute.incrementStart = Number(valueArr[0]);
          this.minute.incrementIncrement = Number(valueArr[1]);
          return;
        }
        if (
          value.indexOf(",") !== -1 ||
          (value.length > 0 &&
            !Number.isNaN(Number(value)) &&
            Number(value) < 60 &&
            Number(value) >= 0)
        ) {
          this.minute.cronEvery = "3";
          const valueArr = value.split(",");
          this.minute.specificSpecific = valueArr.map(item => Number(item));
          return;
        }
        if (value.indexOf("-") !== -1) {
          this.minute.cronEvery = "4";
          const valueArr = value.split("-");
          this.minute.rangeStart = Number(valueArr[0]);
          this.minute.rangeEnd = Number(valueArr[1]);
          return;
        }
      }
    },
    hoursText: {
      get() {
        let hours = "";
        const cronEvery = this.hour.cronEvery;
        switch (cronEvery.toString()) {
          case "1":
            hours = "*";
            break;
          case "2":
            hours =
              this.hour.incrementStart + "/" + this.hour.incrementIncrement;
            break;
          case "3":
            this.hour.specificSpecific.map(val => {
              hours += val + ",";
            });
            hours = hours.slice(0, -1);
            break;
          case "4":
            hours = this.hour.rangeStart + "-" + this.hour.rangeEnd;
            break;
        }
        return hours;
      },
      set(value) {
        if (!value) return;
        if (value === "*") {
          this.hour.cronEvery = "1";
          return;
        }
        if (value.indexOf("/") !== -1) {
          this.hour.cronEvery = "2";
          const valueArr = value.split("/");
          this.hour.incrementStart = Number(valueArr[0]);
          this.hour.incrementIncrement = Number(valueArr[1]);
          return;
        }
        if (
          value.indexOf(",") !== -1 ||
          (value.length > 0 &&
            !Number.isNaN(Number(value)) &&
            Number(value) < 24 &&
            Number(value) >= 0)
        ) {
          this.hour.cronEvery = "3";
          const valueArr = value.split(",");
          this.hour.specificSpecific = valueArr.map(item => Number(item));
          return;
        }
        if (value.indexOf("-") !== -1) {
          this.hour.cronEvery = "4";
          const valueArr = value.split("-");
          this.hour.rangeStart = Number(valueArr[0]);
          this.hour.rangeEnd = Number(valueArr[1]);
          return;
        }
      }
    },
    daysText: {
      get() {
        let days = "";
        const cronEvery = this.day.cronEvery;
        switch (cronEvery.toString()) {
          case "1":
            break;
          case "2":
          case "4":
          case "11":
            days = "?";
            break;
          case "3":
            days = this.day.incrementStart + "/" + this.day.incrementIncrement;
            break;
          case "5":
            this.day.specificSpecific.map(val => {
              days += val + ",";
            });
            days = days.slice(0, -1);
            break;
          case "6":
            days = "L";
            break;
          case "7":
            days = "LW";
            break;
          case "8":
            days = this.day.cronLastSpecificDomDay + "L";
            break;
          case "9":
            days = "L-" + this.day.cronDaysBeforeEomMinus;
            break;
          case "10":
            days = this.day.cronDaysNearestWeekday + "W";
            break;
        }
        return days;
      },
      set(value) {
        if (!value) return;
        // if(value === '*') {
        //   this.day.cronEvery = '1'
        // }
        if (value.indexOf("/") !== -1) {
          this.day.cronEvery = "3";
          const valueArr = value.split("/");
          this.day.incrementStart = Number(valueArr[0]);
          this.day.incrementIncrement = Number(valueArr[1]);
          return;
        }
        if (
          value.indexOf(",") !== -1 ||
          (value.length > 0 &&
            !Number.isNaN(Number(value)) &&
            Number(value) <= 31 &&
            Number(value) > 0)
        ) {
          this.day.cronEvery = "5";
          const valueArr = value.split(",");
          this.day.specificSpecific = valueArr.map(item => Number(item));
          return;
        }
        if (value === "L") {
          this.day.cronEvery = "6";
          return;
        }
        if (value === "LW") {
          this.day.cronEvery = "7";
          return;
        }
        if (/^[1,2,3,4,5,6,7]L$/.test(value)) {
          this.day.cronEvery = "8";
          this.day.cronLastSpecificDomDay = Number(value[0]);
          return;
        }
        if (value.indexOf("L-") !== -1) {
          this.day.cronEvery = "9";
          this.day.cronDaysBeforeEomMinus = Number(value.split("-")[1]);
          return;
        }
        if (/^\dW$/.test(value)) {
          this.day.cronEvery = "10";
          this.day.cronDaysNearestWeekday = Number(value.slice(0, -1));
          return;
        }
      }
    },
    weeksText: {
      get() {
        let weeks = "";
        const cronEvery = this.day.cronEvery;
        switch (cronEvery.toString()) {
          case "1":
          case "3":
          case "5":
            weeks = "?";
            break;
          case "2":
            weeks =
              this.week.incrementStart + "/" + this.week.incrementIncrement;
            break;
          case "4":
            this.week.specificSpecific.map(val => {
              weeks += val + ",";
            });
            weeks = weeks.slice(0, -1);
            break;
          case "6":
          case "7":
          case "8":
          case "9":
          case "10":
            weeks = "?";
            break;
          case "11":
            weeks = this.week.cronNthDayDay + "#" + this.week.cronNthDayNth;
            break;
        }
        return weeks;
      },
      set(value) {
        if (!value) return;
        if (value.indexOf("/") !== -1) {
          this.day.cronEvery = "2";
          const valueArr = value.split("/");
          this.week.incrementStart = Number(valueArr[0]);
          this.week.incrementIncrement = Number(valueArr[1]);
          return;
        }
        if (
          value.indexOf(",") !== -1 ||
          /(SUN)|(MON)|(TUE)|(WED)|(THU)|(FRI)|(SAT)/.test(value)
        ) {
          this.day.cronEvery = "4";
          const valueArr = value.split(",");
          this.week.specificSpecific = valueArr;
          return;
        }
        if (value.indexOf("#") !== -1) {
          const valueArr = value.split("#");
          this.week.cronNthDayDay = Number(valueArr[0]);
          this.week.cronNthDayNth = Number(valueArr[1]);
          return;
        }
      }
    },
    monthsText: {
      get() {
        let months = "";
        const cronEvery = this.month.cronEvery;
        switch (cronEvery.toString()) {
          case "1":
            months = "*";
            break;
          case "2":
            months =
              this.month.incrementStart + "/" + this.month.incrementIncrement;
            break;
          case "3":
            this.month.specificSpecific.map(val => {
              months += val + ",";
            });
            months = months.slice(0, -1);
            break;
          case "4":
            months = this.month.rangeStart + "-" + this.month.rangeEnd;
            break;
        }
        return months;
      },
      set(value) {
        if (!value) return;
        if (value === "*") {
          this.month.cronEvery = "1";
          return;
        }
        if (value.indexOf("/") !== -1) {
          this.month.cronEvery = "2";
          const valueArr = value.split("/");
          this.month.incrementStart = Number(valueArr[0]);
          this.month.incrementIncrement = Number(valueArr[1]);
          return;
        }
        if (
          value.indexOf(",") !== -1 ||
          (value.length > 0 &&
            !Number.isNaN(Number(value)) &&
            Number(value) <= 12 &&
            Number(value) >= 1)
        ) {
          this.month.cronEvery = "3";
          const valueArr = value.split(",");
          this.month.specificSpecific = valueArr.map(item => Number(item));
          return;
        }
        if (value.indexOf("-") !== -1) {
          this.month.cronEvery = "4";
          const valueArr = value.split("-");
          this.month.rangeStart = Number(valueArr[0]);
          this.month.rangeEnd = Number(valueArr[1]);
          return;
        }
      }
    },
    yearsText: {
      get() {
        let years = "";
        const cronEvery = this.year.cronEvery;
        switch (cronEvery.toString()) {
          case "1":
            years = "*";
            break;
          case "2":
            years =
              this.year.incrementStart + "/" + this.year.incrementIncrement;
            break;
          case "3":
            this.year.specificSpecific.map(val => {
              years += val + ",";
            });
            years = years.slice(0, -1);
            break;
          case "4":
            years = this.year.rangeStart + "-" + this.year.rangeEnd;
            break;
        }
        return years;
      },
      set(value) {
        if (!value) return;
        if (value === "*") {
          this.year.cronEvery = "1";
          return;
        }
        if (value.indexOf("/") !== -1) {
          this.year.cronEvery = "2";
          const valueArr = value.split("/");
          this.year.incrementStart = Number(valueArr[0]);
          this.year.incrementIncrement = Number(valueArr[1]);
          return;
        }
        if (
          value.indexOf(",") !== -1 ||
          (value.length > 0 &&
            !Number.isNaN(Number(value)) &&
            Number(value) <= this.nowYear + 100 &&
            Number(value) >= this.nowYear)
        ) {
          this.year.cronEvery = "3";
          const valueArr = value.split(",");
          this.year.specificSpecific = valueArr.map(item => Number(item));
          return;
        }
        if (value.indexOf("-") !== -1) {
          this.year.cronEvery = "4";
          const valueArr = value.split("-");
          this.year.rangeStart = Number(valueArr[0]);
          this.year.rangeEnd = Number(valueArr[1]);
          return;
        }
      }
    },
    cron() {
      return `${this.secondsText || "*"} ${this.minutesText || "*"} ${
        this.hoursText || "*"
      } ${this.daysText || "*"} ${this.monthsText || "*"} ${
        this.weeksText || "?"
      } ${this.yearsText || "*"}`;
    }
  },
  methods: {
    getValue() {
      return this.cron;
    },
    change() {
      this.$emit("change", this.cron);
      this.close();
    },
    close() {
      this.$emit("close");
      // this.setValue(this.formData);
      // this.cron = this.formData.cron || '* * * * * ? *'
    },
    rest(data) {
      for (const i in data) {
        if (data[i] instanceof Object) {
          this.rest(data[i]);
        } else {
          switch (typeof data[i]) {
            case "object":
              data[i] = [];
              break;
            case "string":
              data[i] = "";
              break;
          }
        }
      }
    },
    setValue(value) {
      const cronArr = value.split(" ");
      this.secondsText = cronArr[0];
      this.minutesText = cronArr[1];
      this.hoursText = cronArr[2];
      this.daysText = cronArr[3];
      this.monthsText = cronArr[4];
      this.weeksText = cronArr[5];
      this.yearsText = cronArr[6];
      if (["*", "?"].includes(cronArr[3]) && ["*", "?"].includes(cronArr[5])) {
        this.day.cronEvery = "1";
      }
    }
    // reset() {
    //     activeTab.value = 'second'
    //     setValue('* * * * * ? *')
    // }
  },
  created() {
    const date = new Date();
    this.nowYear = date.getFullYear();
    this.year.incrementStart = this.nowYear;
  },
  mounted() {
    this.setValue(this.formData);
  }
};
</script>
<style lang="scss" scoped>
#changeContab {
  min-width: 900px;
  .el-tabs {
    box-shadow: none;
  }
  .tabBody {
    .el-row {
      margin: 10px 0;
      .long {
        .el-select {
          width: 350px;
        }
      }
      .el-input-number {
        width: 110px;
      }
    }
  }
  .bottom {
    width: 100%;
    text-align: center;
    margin-top: 5px;
    position: relative;
  }
  .value {
    padding-top: 10px;
    font-size: 15px;
    vertical-align: middle;
  }
  .box-card {
    margin-top: 10px;
  }
}
</style>
