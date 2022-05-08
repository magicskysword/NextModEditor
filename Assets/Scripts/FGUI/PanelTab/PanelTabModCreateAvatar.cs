﻿using System.Linq;

public class PanelTabModCreateAvatar : PanelTabTable
{
    public override void OnAdd()
    {
        base.OnAdd();

        AddTableHeader(new TableInfo(
            "main.modCreateAvatar.id".I18N(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var buff = (ModCreateAvatarData)data;
                return buff.ID.ToString();
            }));

        AddTableHeader(new TableInfo(
            "main.modCreateAvatar.name".I18N(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var buff = (ModCreateAvatarData)data;
                return buff.Name;
            }));

        AddTableHeader(new TableInfo(
            "main.modCreateAvatar.createType".I18N(),
            TableInfo.DEFAULT_GRID_WIDTH,
            data =>
            {
                var buff = (ModCreateAvatarData)data;
                return ModMgr.I.GetCreateAvatarTalentTypeDesc(buff.TalentTypeRelation);
            }));

        AddTableHeader(new TableInfo(
            "main.modCreateAvatar.group".I18N(),
            TableInfo.DEFAULT_GRID_WIDTH * 0.5f,
            data =>
            {
                var buff = (ModCreateAvatarData)data;
                return buff.Group.ToString();
            }));

        AddTableHeader(new TableInfo(
            "main.modCreateAvatar.desc".I18N(),
            TableInfo.DEFAULT_GRID_WIDTH * 3,
            data =>
            {
                var buff = (ModCreateAvatarData)data;
                return buff.Desc;
            }));
    }

    protected override void OnInspectItem(object data)
    {
        Inspector.Clear();

        if (data == null)
        {
            return;
        }

        var createAvatarData = (ModCreateAvatarData)data;

        Inspector.AddDrawer(new ModIDPropertyDrawer(
                "main.modCreateAvatar.id".I18N(),
                createAvatarData,
                () => Project.CreateAvatarData,
                theData =>
                {
                    var curData = (ModCreateAvatarData)theData;
                    return $"{curData.ID} {curData.Name}";
                },
                (theData, newId) =>
                {
                    Project.CreateAvatarSeidDataGroup.ChangeSeidID(theData.ID, newId);
                    theData.ID = newId;
                    Project.CreateAvatarData.ModSort();
                    CurInspectIndex = Project.CreateAvatarData.FindIndex(modData => modData == theData);
                },
                (theData, otherData) =>
                {
                    Project.CreateAvatarSeidDataGroup.SwiftSeidID(otherData.ID, theData.ID);
                    (otherData.ID, theData.ID) = (theData.ID, otherData.ID);
                    Project.CreateAvatarData.ModSort();
                    CurInspectIndex = Project.CreateAvatarData.FindIndex(modData => modData == theData);
                },
                () => { Inspector.Refresh(); })
            { OnChanged = RefreshTable }
        );

        Inspector.AddDrawer(new ModStringPropertyDrawer(
                "main.modCreateAvatar.name".I18N(),
                str => createAvatarData.Name = str,
                () => createAvatarData.Name)
            { OnChanged = RefreshCurrentRow }
        );

        Inspector.AddDrawer(new ModIntPropertyDrawer(
                "main.modCreateAvatar.group".I18N(),
                value => createAvatarData.Group = value,
                () => createAvatarData.Group)
            { OnChanged = RefreshCurrentRow }
        );

        Inspector.AddDrawer(new ModIntPropertyDrawer(
            "main.modCreateAvatar.cost".I18N(),
            value => createAvatarData.Cost = value,
            () => createAvatarData.Cost)
        );

        Inspector.AddDrawer(new ModDropdownPropertyDrawer(
            "main.modCreateAvatar.createType".I18N(),
            () => ModMgr.I.CreateAvatarDataTalentTypes.Select(type => type.Desc),
            index => createAvatarData.SetTalentType(ModMgr.I.CreateAvatarDataTalentTypes[index]),
            () => ModMgr.I.CreateAvatarDataTalentTypes.FindIndex(type =>
                type.TypeID == createAvatarData.TalentTypeRelation))
        );
    }

    protected override object GetData(int index)
    {
        return Project.CreateAvatarData[index];
    }

    protected override int GetDataCount()
    {
        return Project.CreateAvatarData.Count;
    }
}