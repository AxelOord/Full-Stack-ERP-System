"use client"

import * as React from "react"
import {
  BookOpen,
  Package,
  User,
  ClipboardList,
  Settings2
} from "lucide-react"
import Image from "next/image"
import { useRefresh } from "@/context/refresh-context";
import { NavMain } from "@/components/sidebar/nav-main"
import { NavAdmin } from "@/components/sidebar/nav-admin"
import { NavSecondary } from "@/components/sidebar/nav-secondary"
import { NavUser } from "@/components/sidebar/nav-user"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar"

const data = {
  user: {
    name: "Axel Oord",
    email: "a.oord@vanderlelie.nl",
    avatar: "https://github.com/shadcn.png",
  },
  navMain: [
    {
      title: "Order overzicht",
      url: "/order-overview",
      icon: Package,
    },
    {
      title: "Magazijn",
      icon: ClipboardList,
      items: [
        {
          title: "Voorraad overzicht",
          url: "/stock-overview",
          isActive: true,
        },
        {
          title: "Verwachte voorraad",
          url: "/expected-stock",
        },
        {
          title: "Voorraad verplaatsen",
          url: "/move-stock",
        },
      ],
    },
    {
      title: "Inkomende stoffen",
      icon: BookOpen,
    },
  ],
  admin: [
    {
      name: "Klanten",
      url: "#",
      icon: User,
    },
    {
      name: "Instellingen",
      url: "#",
      icon: Settings2,
    },
  ],
}

export function AppSidebar({
  ...props
}: {} & React.ComponentPropsWithoutRef<typeof Sidebar>) {
  const { triggerRefresh } = useRefresh();

  return (
    <Sidebar 
      variant="inset" 
      collapsible="icon"
      {...props}
    >
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton size="lg" asChild>
              <a href="#">
                <div className="flex aspect-square size-8 items-center justify-center rounded-full text-sidebar-primary-foreground">
                  <Image src="/logo.png" width='200' height='200' alt="logo" className="size-8" />
                </div>
                <div className="grid flex-1 text-left text-sm leading-tight">
                  <span className="truncate text-xs">Organisatie</span>
                  <span className="truncate font-semibold">Van der Lelie</span>
                </div>
              </a>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>
      <SidebarContent>
        <NavMain items={data.navMain} />
        <NavAdmin projects={data.admin} />
        <NavSecondary triggerRefresh={triggerRefresh} className="mt-auto" />
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={data.user} />
      </SidebarFooter>
    </Sidebar>
  )
}