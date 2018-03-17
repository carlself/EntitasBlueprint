namespace Entitas.Serialization
{
    public class Blueprint
    {
        private string m_name;
        private readonly ComponentBlueprint[] m_components;

        public Blueprint(string name, ComponentBlueprint[] componentBlueprints)
        {
            m_name = name;
            m_components = componentBlueprints;
        }

        public void Apply(Entitas.Entity entity)
        {
            foreach (var component in m_components)
            {
                component.Apply(entity);
            }
        }
    }
}