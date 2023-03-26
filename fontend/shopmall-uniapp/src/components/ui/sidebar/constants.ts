import type { InjectionKey, Ref, ExtractPropTypes } from 'vue'

import type sidebarItemProps from '../sidebar-item/index.vue';


export type SidebarItemStates = {
    hover: boolean
    translate: number
    scale: number
    active: boolean
    ready: boolean
    inStage: boolean
    animating: boolean
}
type SidebarItemProps = ExtractPropTypes<typeof sidebarItemProps>

export type SidebarItemContext = {
    props: SidebarItemProps
    // states: SidebarItemStates
    uid: number
    translateItem: (index: number, activeIndex: number, oldIndex?: number) => void
}

export type SidebarContext = {
    root: Ref<HTMLElement | undefined>
    items: Ref<SidebarItemContext[]>
    loop: boolean
    addItem: (item: SidebarItemContext) => void
    removeItem: (uid: number) => void
    setActiveItem: (index: number) => void
}

export const sidebarContextKey: InjectionKey<SidebarContext> =
    Symbol('sidebarContextKey')