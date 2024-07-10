import { AbstractControl } from "@angular/forms";

export function MustMatch(controlName: string, matchingControlName: string) {
  return (grp: AbstractControl) => {
    const ctrl = grp.get(controlName);
    const matchCtrl = grp.get(matchingControlName);

    if (!ctrl || !matchCtrl) {
      return null;
    }

    if (matchCtrl.errors && !matchCtrl.errors['mustMatch']) {
      return null;
    }

    if (ctrl.value !== matchCtrl.value) {
      matchCtrl.setErrors({ mustMatch: true });
    }
    else {
      matchCtrl.setErrors(null);
    }

    return null;
  }
}
