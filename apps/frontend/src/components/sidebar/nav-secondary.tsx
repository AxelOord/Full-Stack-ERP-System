import * as React from "react"
import { RotateCw, Send, type LucideIcon } from "lucide-react"

import {
  SidebarGroup,
  SidebarGroupContent,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar"

export function NavSecondary({
  triggerRefresh,
  ...props
}: {
  triggerRefresh: () => void
} & React.ComponentPropsWithoutRef<typeof SidebarGroup>) {
  const items = [
    {
      title: "Ververs",
      onClick: () => {
        triggerRefresh()
      },
      icon: RotateCw,
    },
    {
      title: "Feedback",
      onClick: () => {
        console.log("Feedback clicked");
      },
      icon: Send,
    },
  ]

  return (
    <SidebarGroup {...props}>
      <SidebarGroupContent>
        <SidebarMenu>
          {items.map((item) => (
            <SidebarMenuItem key={item.title}>
              <SidebarMenuButton
                asChild
                size="sm"
                onClick={(e) => {
                  e.preventDefault();
                  item.onClick?.();
                }}
              >
                <a className="cursor-pointer">
                  <item.icon />
                  <span>{item.title}</span>
                </a>
              </SidebarMenuButton>
            </SidebarMenuItem>
          ))}
        </SidebarMenu>
      </SidebarGroupContent>
    </SidebarGroup>
  )
}
