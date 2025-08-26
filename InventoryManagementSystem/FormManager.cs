using System;
using System.Collections.Generic;
using System.Windows.Forms;

public static class FormManager
{
    private static Dictionary<Type, Form> openForms = new Dictionary<Type, Form>();

    public static void OpenForm(Form currentForm, Type nextFormType)
    {
        // Hide current form
        currentForm.Hide();

        // Check if form already exists
        if (!openForms.ContainsKey(nextFormType) || openForms[nextFormType].IsDisposed)
        {
            // Create and store it
            Form nextForm = (Form)Activator.CreateInstance(nextFormType);
            nextForm.WindowState = FormWindowState.Maximized;
            openForms[nextFormType] = nextForm;
        }

        // Show the stored form
        openForms[nextFormType].Show();
    }
}
