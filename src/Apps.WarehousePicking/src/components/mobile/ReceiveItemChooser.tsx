import { useState } from "react";
import { ItemPicker } from "@/components/mobile/ItemPicker";
import { NewItemDialog } from "@/components/mobile/NewItemDialog";
import type { PickItem } from "@/hooks/UsePickItems";

type ReceiveItemChooserProps = {
  onSelect: (item: PickItem) => void;
};

// Goods may arrive before an item is in the system, so it can be created from the receive flow.
export function ReceiveItemChooser({ onSelect }: ReceiveItemChooserProps) {
  const [newItemName, setNewItemName] = useState<string | null>(null);

  return (
    <>
      <ItemPicker onSelect={onSelect} onCreate={(search) => setNewItemName(search)} />

      <NewItemDialog
        open={newItemName !== null}
        onOpenChange={(open) => !open && setNewItemName(null)}
        defaults={{ name: newItemName ?? undefined }}
        onCreated={(item) => {
          setNewItemName(null);
          onSelect(item);
        }}
      />
    </>
  );
}
